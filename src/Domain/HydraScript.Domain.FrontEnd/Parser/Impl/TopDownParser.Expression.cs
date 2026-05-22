using System.Globalization;
using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl;

public partial class TopDownParser
{
    /// <summary>
    /// Expression -> CastExpression | AssignmentExpression
    /// AssignmentExpression -> MemberExpression "Assign" Expression
    /// </summary>
    private Expression Expression()
    {
        var expr = CastExpression();
        if (expr is MemberExpression lhs && CurrentIs("Assign"))
        {
            var assign = Expect("Assign");
            var source = assign.Value is "="
                ? Expression()
                : new BinaryExpression(
                    lhs.Empty() ? lhs.Id.Clone() : lhs.Clone(),
                    assign.Value[..^1],
                    Expression());
            return new AssignmentExpression(lhs, source)
                { Segment = assign.Segment };
        }

        return expr;
    }

    /// <summary>
    /// CallExpression -> MemberExpression Arguments
    /// Arguments -> '(' (Expression ',')* ')'
    /// </summary>
    private Expression CallExpression()
    {
        var member = MemberExpression();
        if (CurrentIs("LeftParen"))
        {
            Expect("LeftParen");
            var expressions = new List<Expression>();

            while (CurrentIsExpression())
            {
                expressions.Add(Expression());
                if (!CurrentIs("RightParen"))
                    Expect("Comma");
            }

            var rp = Expect("RightParen");
            return new CallExpression(member, expressions)
                { Segment = member.Segment + rp.Segment };
        }

        return member.Empty() && !CurrentIs("Assign") ? member.Id : member;
    }

    /// <summary>
    /// MemberExpression -> Var ('[' Expression ']' | '.' 'Ident')*
    /// </summary>
    private MemberExpression MemberExpression()
    {
        var memberRoot = Var();
        var accessChain = new LinkedList<AccessExpression>();
        while (CurrentIs("LeftBracket") || CurrentIs("Dot"))
        {
            if (CurrentIs("LeftBracket"))
            {
                var lb = Expect("LeftBracket").Segment;
                var expr = Expression();
                var rb = Expect("RightBracket").Segment;
                accessChain.AddLast(
                    new IndexAccess(expr, accessChain.Last?.Value)
                        { Segment = lb + rb });
            }
            else if (CurrentIs("Dot"))
            {
                var access = Expect("Dot");
                var propToken = Expect("Ident");
                var propIdent = new IdentifierReference(propToken.Value)
                    { Segment = propToken.Segment };
                accessChain.AddLast(
                    new DotAccess(propIdent, accessChain.Last?.Value)
                        { Segment = access.Segment });
            }
        }

        return new MemberExpression(memberRoot, accessChain)
        {
            Segment = memberRoot.Segment
        };
    }

    /// <summary>
    /// CastExpression -> WithExpression 'as' TypeValue
    /// </summary>
    private Expression CastExpression()
    {
        var withExpr = WithExpression();
        if (CurrentIsKeyword("as"))
        {
            var asKeyword = Expect("Keyword", "as");
            var type = TypeValue();
            return new CastAsExpression(withExpr, type) { Segment = asKeyword.Segment };
        }

        return withExpr;
    }

    /// <summary>
    /// WithExpression -> ConditionalExpression 'with' ObjectLiteral
    /// </summary>
    private Expression WithExpression()
    {
        var cond = ConditionalExpression();
        if (CurrentIsKeyword("with"))
        {
            var withKeyword = Expect("Keyword", "with");
            var objectLiteral = ObjectLiteral();
            return new WithExpression(cond, objectLiteral) { Segment = withKeyword.Segment };
        }

        return cond;
    }

    /// <summary>
    /// ConditionalExpression -> OrExpression ('?' Expression ':' Expression)?
    /// </summary>
    private Expression ConditionalExpression()
    {
        var test = OrExpression();
        if (CurrentIs("QuestionMark"))
        {
            Expect("QuestionMark");
            var consequent = Expression();
            Expect("Colon");
            var alternate = Expression();
            return new ConditionalExpression(test, consequent, alternate)
            {
                Segment = consequent.Segment + alternate.Segment
            };
        }

        return test;
    }

    /// <summary>
    /// OrExpression -> AndExpression ('||' AndExpression)*
    /// </summary>
    private Expression OrExpression()
    {
        var left = AndExpression();
        while (CurrentIsOperator("||"))
        {
            var op = Expect("Operator");
            var right = AndExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    /// <summary>
    /// AndExpression -> EqExpression ('&&' EqExpression)*
    /// </summary>
    private Expression AndExpression()
    {
        var left = EqualityExpression();
        while (CurrentIsOperator("&&"))
        {
            var op = Expect("Operator");
            var right = EqualityExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    /// <summary>
    /// EqExpression -> RelExpression (('=='|'!=') RelExpression)*
    /// </summary>
    private Expression EqualityExpression()
    {
        var left = RelationExpression();
        while (CurrentIsOperator("==") || CurrentIsOperator("!="))
        {
            var op = Expect("Operator");
            var right = RelationExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    /// <summary>
    /// RelExpression -> AddExpression (('&lt;'|'&gt;'|'&#x2264;'|'&#x2265;') AddExpression)*
    /// </summary>
    private Expression RelationExpression()
    {
        var left = AdditiveExpression();
        while (CurrentIsOperator(">") || CurrentIsOperator("<") ||
               CurrentIsOperator(">=") || CurrentIsOperator("<="))
        {
            var op = Expect("Operator");
            var right = AdditiveExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    /// <summary>
    /// AddExpression -> MulExpression (('+'|'-') MulExpression)*
    /// </summary>
    private Expression AdditiveExpression()
    {
        var left = MultiplicativeExpression();
        while (CurrentIsOperator("+") || CurrentIsOperator("-"))
        {
            var op = Expect("Operator");
            var right = MultiplicativeExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    /// <summary>
    /// MulExpression -> UnaryExpression (('*'|'/'|'%'|'++'|'::') UnaryExpression)*
    /// </summary>
    private Expression MultiplicativeExpression()
    {
        var left = UnaryExpression();
        while (CurrentIsOperator("*") || CurrentIsOperator("/") || CurrentIsOperator("%")
               || CurrentIsOperator("++") || CurrentIsOperator("::"))
        {
            var op = Expect("Operator");
            var right = UnaryExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    /// <summary>
    /// UnaryExpression -> LeftHandSideExpression | ('-'|'!'|'~') UnaryExpression
    /// </summary>
    private Expression UnaryExpression()
    {
        if (CurrentIsUnaryOperator(expectEnv: false))
        {
            var op = Expect("Operator");
            return new UnaryExpression(op.Value, UnaryExpression())
            {
                Segment = op.Segment
            };
        }

        return LeftHandSideExpression();
    }

    /// <summary>
    /// LeftHandSideExpression -> PrimaryExpression
    ///                           ParenthesizedExpression
    ///                           ComplexLiteral
    ///                           MemberExpression
    ///                           CallExpression
    /// ParenthesizedExpression -> '(' Expression ')'
    /// </summary>
    private Expression LeftHandSideExpression()
    {
        if (CurrentIs("LeftParen"))
        {
            Expect("LeftParen");
            var expr = Expression();
            Expect("RightParen");
            return expr;
        }

        if (CurrentIs("LeftCurl") || CurrentIs("LeftBracket"))
        {
            return ComplexLiteral();
        }

        if (CurrentIs("Ident") || CurrentIsOperator("$"))
        {
            return CallExpression();
        }

        return PrimaryExpression();
    }

    /// <summary>
    /// PrimaryExpression -> Var | Literal
    /// </summary>
    private PrimaryExpression PrimaryExpression()
    {
        return LiteralNode();
    }

    /// <summary>
    /// Var -> "Ident" | EnvVar
    /// EnvVar -> '$' "Ident"
    /// </summary>
    private IdentifierReference Var()
    {
        if (CurrentIs("Ident"))
        {
            var ident = Expect("Ident");
            return new IdentifierReference(ident.Value)
            {
                Segment = ident.Segment
            };
        }

        var dollar = Expect("Operator");
        var envIdent = Expect("Ident");
        return new EnvVarReference(envIdent.Value)
        {
            Segment = dollar.Segment + envIdent.Segment
        };
    }

    /// <summary>
    /// Literal -> "NullLiteral"
    ///            "IntegerLiteral"
    ///            "FloatLiteral"
    ///            "StringLiteral"
    ///            "BooleanLiteral"
    /// </summary>
    private Literal LiteralNode()
    {
        var segment = _tokens.Current.Segment;
        if (CurrentIs("StringLiteral"))
        {
            var str = Expect("StringLiteral");
            return Literal.String(
                value: Regex.Unescape(str.Value.Trim('"')),
                segment,
                label: str.Value
                    .Replace(@"\", @"\\")
                    .Replace(@"""", @"\"""));
        }

        if (CurrentIs("NullLiteral"))
        {
            Expect("NullLiteral");
            return Literal.Null(segment);
        }

        return _tokens.Current.Type.Tag switch
        {
            "IntegerLiteral" => Literal.Number(value: double.Parse(Expect("IntegerLiteral").Value), segment),
            "FloatLiteral" => Literal.Number(
                value: double.Parse(
                    Expect("FloatLiteral").Value,
                    CultureInfo.InvariantCulture),
                segment),
            "BooleanLiteral" => Literal.Boolean(value: bool.Parse(Expect("BooleanLiteral").Value), segment),
            _ => throw new ParserException("Literal", _tokens.Current)
        };
    }

    /// <summary>
    /// ComplexLiteral -> ObjectLiteral | ArrayLiteral
    /// </summary>
    private ComplexLiteral ComplexLiteral()
    {
        if (CurrentIs("LeftCurl"))
        {
            return ObjectLiteral();
        }

        return ArrayLiteral();
    }

    /// <summary>
    /// ObjectLiteral -> '{' PropertyDefinitionList '}'
    /// PropertyDefinitionList -> (FieldProperty ';')*
    /// FieldProperty -> "Ident" ':' Expression
    /// </summary>
    private ObjectLiteral ObjectLiteral()
    {
        Expect("LeftCurl");
        var properties = new List<Property>();
        while (CurrentIs("Ident"))
        {
            var idToken = Expect("Ident");
            var id = new IdentifierReference(idToken.Value)
                { Segment = idToken.Segment };

            Expect("Colon");
            var expr = Expression();
            properties.Add(new Property(id, expr) { Segment = idToken.Segment });

            Expect("SemiColon");
        }

        Expect("RightCurl");
        return new ObjectLiteral(properties);
    }

    /// <summary>
    /// ArrayLiteral -> '[' ElementList ']'
    /// ElementList -> (Expression ',')*
    /// </summary>
    private ArrayLiteral ArrayLiteral()
    {
        var lb = Expect("LeftBracket").Segment;
        var expressions = new List<Expression>();
        while (CurrentIsExpression())
        {
            expressions.Add(Expression());
            if (!CurrentIs("RightBracket"))
            {
                Expect("Comma");
            }
        }

        var rb = Expect("RightBracket").Segment;
        return new ArrayLiteral(expressions) { Segment = lb + rb };
    }
}
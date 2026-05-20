using System.Globalization;
using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl;

public partial class TopDownParser
{
    /// <summary>
    /// Expression -> CastExpression | AssignmentExpression
    /// </summary>
    private Expression Expression()
    {
        var expr = CastExpression();
        if (expr is LeftHandSideExpression lhs && CurrentIs("Assign"))
        {
            var assign = Expect("Assign");
            return new AssignmentExpression(lhs, Expression())
                { Segment = assign.Segment };
        }

        return expr;
    }

    /// <summary>
    /// CallExpression -> MemberExpression Arguments (Arguments | '[' Expression ']' | '.' 'Ident')*
    /// </summary>
    private Expression CallExpression()
    {
        var member = MemberExpression();
        if (CurrentIs("LeftParen"))
        {
            Expect("LeftParen");
            var expressions = new List<Expression>();
            if (CurrentIsExpression())
            {
                expressions.Add(Expression());
            }

            while (CurrentIs("Comma"))
            {
                Expect("Comma");
                expressions.Add(Expression());
            }

            var rp = Expect("RightParen");
            return new CallExpression((member as MemberExpression)!, expressions)
                { Segment = member.Segment + rp.Segment };
        }

        return member;
    }

    /// <summary>
    /// MemberExpression -> "Ident" ('[' Expression ']' | '.' 'Ident')*
    /// </summary>
    private Expression MemberExpression()
    {
        var primary = PrimaryExpression();

        if (!CurrentIs("LeftBracket") && !CurrentIs("Dot") &&
            !CurrentIs("Assign") && !CurrentIs("LeftParen"))
            return primary;

        var identRef = (primary as IdentifierReference)!;
        var accessChain = new List<AccessExpression>();
        while (CurrentIs("LeftBracket") || CurrentIs("Dot"))
        {
            Token access;
            if (CurrentIs("LeftBracket"))
            {
                access = Expect("LeftBracket");
                var lb = access.Segment;
                var expr = Expression();
                var rb = Expect("RightBracket").Segment;
                accessChain.Add(
                    new IndexAccess(expr, accessChain.LastOrDefault()) { Segment = lb + rb });
            }
            else if (CurrentIs("Dot"))
            {
                access = Expect("Dot");
                var identToken = Expect("Ident");
                var idRef = new IdentifierReference(identToken.Value)
                    { Segment = identToken.Segment };
                accessChain.Add(
                    new DotAccess(idRef, accessChain.LastOrDefault()) { Segment = access.Segment });
            }
        }

        return new MemberExpression(
            identRef,
            accessChain.FirstOrDefault(),
            tail: accessChain.LastOrDefault())
        {
            Segment = identRef.Segment
        };
    }

    /// <summary>
    /// CastExpression -> WithExpression 'as' 'string'
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
        while (CurrentIsOperator(">") || CurrentIsOperator("<") || CurrentIsOperator(">=") ||
               CurrentIsOperator("<="))
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
    /// LeftHandSideExpression -> MemberExpression | CallExpression
    /// </summary>
    private Expression LeftHandSideExpression()
    {
        return CallExpression();
    }

    /// <summary>
    /// PrimaryExpression -> "Ident" | EnvVar | Literal | '(' Expression ')' | ObjectLiteral | ArrayLiteral
    /// EnvVar -> '$' "Ident"
    /// </summary>
    private Expression PrimaryExpression()
    {
        if (CurrentIs("LeftParen"))
        {
            Expect("LeftParen");
            var expr = Expression();
            Expect("RightParen");
            return expr;
        }

        if (CurrentIs("Ident"))
        {
            var ident = Expect("Ident");
            return new IdentifierReference(ident.Value)
            {
                Segment = ident.Segment
            };
        }

        if (CurrentIsOperator("$"))
        {
            var dollar = Expect("Operator");
            var ident = Expect("Ident");
            return new EnvVarReference(ident.Value)
            {
                Segment = dollar.Segment + ident.Segment
            };
        }

        if (CurrentIsLiteral())
        {
            return LiteralNode();
        }

        if (CurrentIs("LeftCurl"))
        {
            return ObjectLiteral();
        }

        if (CurrentIs("LeftBracket"))
        {
            return ArrayLiteral();
        }

        return null!;
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
            _ => throw new ParserException("There are no more supported literals")
        };
    }

    /// <summary>
    /// ObjectLiteral -> '{' PropertyDefinitionList '}'
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
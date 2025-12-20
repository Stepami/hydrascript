using System.Globalization;
using System.Text.RegularExpressions;
using HydraScript.Domain.Constants;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.Domain.FrontEnd.Parser.Impl;

public class TopDownParser(ILexer lexer) : IParser
{
    private IEnumerator<Token> _tokens = Enumerable.Empty<Token>().GetEnumerator();

    public IAbstractSyntaxTree Parse(string text)
    {
        _tokens = lexer.GetTokens(text).GetEnumerator();
        _tokens.MoveNext();

        var root = Script();
        Expect(Eop.Tag);
        return new AbstractSyntaxTree(root);
    }

    private Token Expect(string expectedTag, string? expectedValue = null)
    {
        var current = _tokens.Current;

        if (!CurrentIs(expectedTag))
            throw new ParserException(_tokens.Current.Segment, expectedTag, _tokens.Current);
        if (_tokens.Current.Value != (expectedValue ?? _tokens.Current.Value))
            throw new ParserException(_tokens.Current.Segment, expectedValue, _tokens.Current);

        _tokens.MoveNext();
        return current;
    }

    private bool CurrentIs(string tag) =>
        _tokens.Current.Type == lexer.Structure.FindByTag(tag);

    private bool CurrentIsLiteral() =>
        CurrentIs("NullLiteral") ||
        CurrentIs("IntegerLiteral") ||
        CurrentIs("FloatLiteral") ||
        CurrentIs("StringLiteral") ||
        CurrentIs("BooleanLiteral");

    private bool CurrentIsKeyword(string keyword) =>
        CurrentIs("Keyword") &&
        _tokens.Current.Value == keyword;

    private bool CurrentIsOperator(string @operator) =>
        CurrentIs("Operator") &&
        _tokens.Current.Value == @operator;

    private bool CurrentIsUnaryOperator(bool expectEnv = true) =>
        CurrentIsOperator("-") || CurrentIsOperator("!") ||
        CurrentIsOperator("~") || (expectEnv && CurrentIsOperator("$"));

    private bool CurrentIsDeclaration() =>
        CurrentIsKeyword("function") || CurrentIsKeyword("let") ||
        CurrentIsKeyword("const") || CurrentIsKeyword("type");

    private bool CurrentIsExpression() =>
        CurrentIs("Ident") || CurrentIsLiteral() || CurrentIsUnaryOperator() ||
        CurrentIs("LeftParen") || CurrentIs("LeftCurl") || CurrentIs("LeftBracket");

    /// <summary>
    /// Script -> StatementList
    /// </summary>
    private ScriptBody Script() =>
        new(StatementList());

    /// <summary>
    /// StatementList -> StatementListItem*
    /// </summary>
    private List<StatementListItem> StatementList()
    {
        var statementList = new List<StatementListItem>();
        while (CurrentIsDeclaration() || CurrentIsExpression() ||
               CurrentIs("Output") || CurrentIs("Input") ||
               CurrentIsKeyword("return") || CurrentIsKeyword("break") || CurrentIsKeyword("continue") ||
               CurrentIsKeyword("if") || CurrentIsKeyword("while"))
        {
            statementList.Add(StatementListItem());
        }

        return statementList;
    }

    /// <summary>
    /// StatementListItem -> Statement | Declaration
    /// </summary>
    private StatementListItem StatementListItem()
    {
        if (CurrentIsDeclaration())
        {
            return Declaration();
        }

        return Statement();
    }

    /// <summary>
    /// Statement -> BlockStatement
    ///              ExpressionStatement
    ///              IfStatement
    ///              WhileStatement
    ///              ContinueStatement
    ///              BreakStatement
    ///              ReturnStatement
    ///              OutputStatement
    ///              InputStatement
    /// </summary>
    private Statement Statement()
    {
        if (CurrentIs("Ident") || CurrentIsLiteral() ||
            CurrentIs("LeftParen") || CurrentIsUnaryOperator())
            return ExpressionStatement();

        if (CurrentIs("LeftCurl"))
            return BlockStatement();

        if (CurrentIsKeyword("return"))
            return ReturnStatement();

        if (CurrentIsKeyword("break"))
            return new InsideStatementJump(InsideStatementJumpKeyword.Break)
            {
                Segment = Expect("Keyword", "break").Segment
            };

        if (CurrentIsKeyword("continue"))
            return new InsideStatementJump(InsideStatementJumpKeyword.Continue)
            {
                Segment = Expect("Keyword", "continue").Segment
            };

        if (CurrentIsKeyword("if"))
            return IfStatement();

        if (CurrentIsKeyword("while"))
            return WhileStatement();

        if (CurrentIs("Output"))
            return OutputStatement();

        if (CurrentIs("Input"))
            return InputStatement();

        return null!;
    }

    /// <summary>
    /// BlockStatement -> '{' StatementList '}'
    /// </summary>
    private BlockStatement BlockStatement()
    {
        Expect("LeftCurl");
        var block = new BlockStatement(StatementList());
        Expect("RightCurl");

        return block;
    }

    /// <summary>
    /// ExpressionStatement -> Expression
    /// </summary>
    private ExpressionStatement ExpressionStatement()
    {
        return new(Expression());
    }

    /// <summary>
    /// ReturnStatement -> 'return' Expression?
    /// </summary>
    private ReturnStatement ReturnStatement()
    {
        var ret = Expect("Keyword", "return");
        if (CurrentIsExpression())
        {
            return new ReturnStatement(Expression()) { Segment = ret.Segment };
        }

        return new ReturnStatement { Segment = ret.Segment };
    }

    /// <summary>
    /// IfStatement -> 'if' '(' Expression ')' Statement ('else' Statement)?
    /// </summary>
    private IfStatement IfStatement()
    {
        var token = Expect("Keyword", "if");
        Expect("LeftParen");
        var expr = Expression();
        Expect("RightParen");
        var then = Statement();
        if (CurrentIsKeyword("else"))
        {
            Expect("Keyword", "else");
            var @else = Statement();
            return new IfStatement(expr, then, @else) {Segment = token.Segment};
        }

        return new IfStatement(expr, then) {Segment = token.Segment};
    }

    /// <summary>
    /// WhileStatement -> 'while' '(' Expression ')' Statement
    /// </summary>
    private WhileStatement WhileStatement()
    {
        var token = Expect("Keyword", "while");
        Expect("LeftParen");
        var expr = Expression();
        Expect("RightParen");
        var stmt = Statement();
        return new WhileStatement(expr, stmt) {Segment = token.Segment};
    }

    /// <summary>
    /// OutputStatement -> '>>>' Expression
    /// </summary>
    private OutputStatement OutputStatement()
    {
        Expect("Output");
        return new OutputStatement(Expression());
    }

    /// <summary>
    /// InputStatement -> '&lt;&lt;&lt;' (Ident | EnvVar)
    /// </summary>
    private InputStatement InputStatement()
    {
        var input = Expect("Input");
        if (CurrentIsOperator("$"))
        {
            var dollar = Expect("Operator");
            var envIdent = Expect("Ident");
            return new InputStatement(new EnvVarReference(envIdent.Value)
            {
                Segment = dollar.Segment + envIdent.Segment
            })
            {
                Segment = input.Segment + envIdent.Segment
            };
        }

        var ident = Expect("Ident");
        return new InputStatement(new IdentifierReference(ident.Value) { Segment = ident.Segment })
        {
            Segment = input.Segment + ident.Segment
        };
    }

    /// <summary>
    /// TypeDeclaration -> 'type' "Ident" = TypeValue
    /// </summary>
    private TypeDeclaration TypeDeclaration()
    {
        var typeWord = Expect("Keyword", "type");
        var ident = Expect("Ident");
        Expect("Assign");
        var type = TypeValue();

        var typeId = new IdentifierReference(name: ident.Value)
            { Segment = ident.Segment };

        return new TypeDeclaration(typeId, type) { Segment = typeWord.Segment + ident.Segment };
    }

    /// <summary>
    /// TypeValue -> TypeValueBase TypeValueSuffix*
    /// </summary>
    private TypeValue TypeValue()
    {
        if (CurrentIs("Ident"))
        {
            var ident = Expect("Ident");
            var identType = new TypeIdentValue(
                TypeId: new IdentifierReference(name: ident.Value)
                    { Segment = ident.Segment });

            return WithSuffix(identType);
        }

        if (CurrentIs("LeftCurl"))
        {
            Expect("LeftCurl");
            var propertyTypes = new List<PropertyTypeValue>();
            while (CurrentIs("Ident"))
            {
                var ident = Expect("Ident");
                Expect("Colon");
                var propType = TypeValue(); 
                propertyTypes.Add(
                    new PropertyTypeValue(
                        ident.Value,
                        propType));
                Expect("SemiColon");
            }

            Expect("RightCurl");
                
            return WithSuffix(new ObjectTypeValue(propertyTypes));
        }

        return null!;
    }

    /// <summary>
    /// TypeValueSuffix -> '['']' | '?'
    /// </summary>
    private TypeValue WithSuffix(TypeValue baseType)
    {
        var type = baseType;
        while (CurrentIs("LeftBracket") || CurrentIs("QuestionMark"))
        {
            if (CurrentIs("LeftBracket"))
            {
                Expect("LeftBracket");
                Expect("RightBracket");
                type = new ArrayTypeValue(type);
            } 
            else if (CurrentIs("QuestionMark"))
            {
                Expect("QuestionMark");
                type = new NullableTypeValue(type);
            }
        }

        return type;
    }

    /// <summary>
    /// Declaration -> LexicalDeclaration | FunctionDeclaration | TypeDeclaration
    /// </summary>
    private Declaration Declaration()
    {
        if (CurrentIsKeyword("function"))
        {
            return FunctionDeclaration();
        }

        if (CurrentIsKeyword("let") || CurrentIsKeyword("const"))
        {
            return LexicalDeclaration();
        }
        
        if (CurrentIsKeyword("type"))
        {
            return TypeDeclaration();
        }

        return null!;
    }

    /// <summary>
    /// FunctionDeclaration -> 'function' "Ident" '(' FunctionParameters? ')' Type? BlockStatement
    /// </summary>
    private FunctionDeclaration FunctionDeclaration()
    {
        Expect("Keyword", "function");
        var ident = Expect("Ident");

        Expect("LeftParen");
        var args = new List<IFunctionArgument>();
        var indexOfFirstDefaultArgument = int.MaxValue;
        while (CurrentIs("Ident"))
        {
            var arg = Expect("Ident").Value;
            if (CurrentIs("Colon"))
            {
                Expect("Colon");
                var type = TypeValue();
                args.Add(new NamedArgument(arg, type));
            }
            else if (CurrentIs("Assign"))
            {
                Expect("Assign");
                var value = LiteralNode();
                indexOfFirstDefaultArgument = args.Count < indexOfFirstDefaultArgument
                    ? args.Count
                    : indexOfFirstDefaultArgument;
                args.Add(new DefaultValueArgument(arg, value));
            }

            if (!CurrentIs("RightParen"))
                Expect("Comma");
        }

        var rp = Expect("RightParen");

        TypeValue returnType = new TypeIdentValue(
            TypeId: new IdentifierReference(name: "undefined")
                { Segment = rp.Segment });

        if (CurrentIs("Colon"))
        {
            Expect("Colon");
            returnType = TypeValue();
        }

        var name = new IdentifierReference(ident.Value) { Segment = ident.Segment };
        return new FunctionDeclaration(name, returnType, args, BlockStatement(), indexOfFirstDefaultArgument)
            { Segment = ident.Segment };
    }

    /// <summary>
    /// LexicalDeclaration -> LetOrConst "Ident" Initialization (',' "Ident" Initialization)*
    /// </summary>
    private LexicalDeclaration LexicalDeclaration()
    {
        var readOnly = CurrentIsKeyword("const");
        Expect("Keyword", readOnly ? "const" : "let");
        var declaration = new LexicalDeclaration(readOnly);

        AddToDeclaration(declaration);

        while (CurrentIs("Comma"))
        {
            Expect("Comma");
            AddToDeclaration(declaration);
        }

        return declaration;
    }

    /// <summary>
    /// Initialization -> Typed | Initializer
    /// Typed -> Type Initializer?
    /// Initializer -> '=' Expression
    /// </summary>
    private void AddToDeclaration(LexicalDeclaration declaration)
    {
        var ident = Expect("Ident");
        var identRef = new IdentifierReference(ident.Value) { Segment = ident.Segment };
        var assignment = new AssignmentExpression(
                new MemberExpression(identRef),
                new ImplicitLiteral(TypeIdentValue.Undefined))
            { Segment = ident.Segment };

        if (CurrentIs("Assign"))
        {
            var assignSegment = Expect("Assign").Segment;
            var expression = Expression();
            assignment = new AssignmentExpression(
                new MemberExpression(identRef), expression
            ) { Segment = assignSegment };
        }
        else if (CurrentIs("Colon"))
        {
            Expect("Colon");
            var type = TypeValue();
            if (CurrentIs("Assign"))
            {
                var assignSegment = Expect("Assign").Segment;
                var expression = Expression();
                assignment = new AssignmentExpression(
                    new MemberExpression(identRef),
                    expression, type
                ) { Segment = assignSegment };
            }
            else
            {
                var expression = new ImplicitLiteral(type);
                assignment = new AssignmentExpression(
                    lhs: new MemberExpression(identRef),
                    expression,
                    type);
            }
        }
        declaration.AddAssignment(assignment);
    }

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
                {Segment = assign.Segment};
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
                    new IndexAccess(expr, accessChain.LastOrDefault()) {Segment = lb + rb}
                );
            }
            else if (CurrentIs("Dot"))
            {
                access = Expect("Dot");
                var identToken = Expect("Ident");
                var idRef = new IdentifierReference(identToken.Value)
                    { Segment = identToken.Segment };
                accessChain.Add(
                    new DotAccess(idRef, accessChain.LastOrDefault()) {Segment = access.Segment}
                );
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
            return new CastAsExpression(withExpr, type) {Segment = asKeyword.Segment};
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
            return new WithExpression(cond, objectLiteral) {Segment = withKeyword.Segment};
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
            var dollar =  Expect("Operator");
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
        return new ArrayLiteral(expressions) {Segment = lb + rb};
    }
}
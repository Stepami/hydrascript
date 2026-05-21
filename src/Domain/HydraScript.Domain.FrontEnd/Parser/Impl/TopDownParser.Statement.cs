using HydraScript.Domain.Constants;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.Domain.FrontEnd.Parser.Impl;

public partial class TopDownParser
{
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

        throw new ParserException(nameof(Statement), _tokens.Current);
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
            return new IfStatement(expr, then, @else) { Segment = token.Segment };
        }

        return new IfStatement(expr, then) { Segment = token.Segment };
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
        return new WhileStatement(expr, stmt) { Segment = token.Segment };
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
            return new InputStatement(
                new EnvVarReference(envIdent.Value)
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
}
using HydraScript.Domain.Constants;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;

namespace HydraScript.Domain.FrontEnd.Parser.Impl;

public partial class TopDownParser(ILexer lexer) : IParser
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
            throw new ParserException(expectedTag, _tokens.Current);
        if (_tokens.Current.Value != (expectedValue ?? _tokens.Current.Value))
            throw new ParserException(expectedValue, _tokens.Current);

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

    private bool CurrentIsStatement() =>
        CurrentIsExpression() ||
        CurrentIs("Output") || CurrentIs("Input") ||
        CurrentIsKeyword("return") || CurrentIsKeyword("break") || CurrentIsKeyword("continue") ||
        CurrentIsKeyword("if") || CurrentIsKeyword("while");

    /// <summary>
    /// Script -> StatementList
    /// </summary>
    private ScriptBody Script() => new(StatementList());

    /// <summary>
    /// StatementList -> StatementListItem*
    /// </summary>
    private List<StatementListItem> StatementList()
    {
        var statementList = new List<StatementListItem>();
        while (CurrentIsDeclaration() || CurrentIsStatement())
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
            return Declaration();

        return Statement();
    }
}
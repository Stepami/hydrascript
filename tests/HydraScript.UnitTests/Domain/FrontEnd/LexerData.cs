namespace HydraScript.UnitTests.Domain.FrontEnd;

public sealed class LexerSuccessData : TheoryData<string>
{
    public LexerSuccessData()
    {
        Add("a + b - c return while do");
        Add("=> abc null true false");
        Add("{ . } , ( ) [] =?:");
    }
}

public sealed class LexerFailData : TheoryData<string>
{
    public LexerFailData()
    {
        Add("a + v ```");
        Add("kkk &");
        Add("|| |");
    }
}

public sealed class LexerKeywordInsideIdentData : TheoryData<string>
{
    public LexerKeywordInsideIdentData()
    {
        Add("constExpr");
        Add("letVarConst");
        Add("nullObj");
        Add("trueStmt");
        Add("falseStmt");
        Add("returnResult");
    }
}
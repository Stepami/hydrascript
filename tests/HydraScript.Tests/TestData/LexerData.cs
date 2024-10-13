using Xunit;

namespace HydraScript.Tests.TestData;

public class LexerSuccessData : TheoryData<string>
{
    public LexerSuccessData()
    {
        Add("a + b - c return while do");
        Add("=> abc null true false");
        Add("{ . } , ( ) [] =?:");
    }
}

public class LexerFailData : TheoryData<string>
{
    public LexerFailData()
    {
        Add("a + v $$$");
        Add("kkk &");
        Add("|| |");
    }
}
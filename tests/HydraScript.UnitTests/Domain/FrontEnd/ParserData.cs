namespace HydraScript.UnitTests.Domain.FrontEnd;

public sealed class ParserSuccessTestData : TheoryData<string>
{
    public ParserSuccessTestData()
    {
        Add("-21");
        Add("!false");
        Add("~[)");
        Add("x = ([1,2) ++ [3,4))::0");
        Add("i[0).j");
        Add("i[0).j()");
        Add("i = 1");
        Add("i[0) = 1");
        Add("i[a.b)[1).x(1)");
        Add("(1 + 2) * (3 - (2 / 2)) as string");
        Add("return {x:1;y:2;}");
        Add("while (~arr != 0) { arr::0 continue }");
        Add("if (!(true || (false && false))) { break } else { break }");
    }
}
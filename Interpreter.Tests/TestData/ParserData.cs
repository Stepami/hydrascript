using System.Collections;

namespace Interpreter.Tests.TestData;

public class ParserSuccessTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "-21" };
        yield return new object[] { "!false" };
        yield return new object[] { "~[]" };
        yield return new object[] { "x = ([1,2] ++ [3,4])::0" };
        yield return new object[] {"i[0].j"};
        yield return new object[] {"i[0].j()"};
        yield return new object[] {"i = 1"};
        yield return new object[] {"i[0] = 1"};
        yield return new object[] {"i[a.b][1].x(1)"};
        yield return new object[] {"(1 + 2) * (3 - (2 / 2)) as string"};
        yield return new object[] { "return {x:1;y:2;}" };
        yield return new object[] { "while (~arr != 0) { arr::0 continue }" };
        yield return new object[] { "if (!(true || (false && false))) { break } else { break }" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
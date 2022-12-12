using System.Collections;

namespace Interpreter.Tests.TestData
{
    public class ParserSuccessTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {"i[0].j"};
            yield return new object[] {"i[0].j()"};
            yield return new object[] {"i = 1"};
            yield return new object[] {"i[0] = 1"};
            yield return new object[] {"i[a.b][1].x(1)"};
            yield return new object[] {"(1 + 2) * (3 - (2 / 2)) as string"};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
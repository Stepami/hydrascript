using System.Collections;
using System.Collections.Generic;

namespace Interpreter.Tests.TestData
{
    public class ParserTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {"i[0].j"};
            yield return new object[] {"i[0].j()"};
            yield return new object[] {"i = 1"};
            yield return new object[] {"i[0] = 1"};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
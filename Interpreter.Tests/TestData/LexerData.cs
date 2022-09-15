using System.Collections;
using System.Collections.Generic;

namespace Interpreter.Tests.TestData
{
    public class LexerSuccessData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "a + b - c return while do" };
            yield return new object[] { "=> abc null true false" };
            yield return new object[] { "{ . } , ( ) [] =?:" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class LexerFailData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "a + v $$$" };
            yield return new object[] { "kkk &" };
            yield return new object[] { "|| |" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
using System.Collections.Generic;
using Interpreter.Lib.BackEnd;
using Xunit;

namespace Interpreter.Tests.Unit.BackEnd
{
    public class CallTests
    {
        [Fact]
        public void ToStringCorrect()
        {
            var call = new Call(9, new FunctionInfo("func"),
                new List<(string Id, object Value)>
                {
                    ("arg", 1)
                }
            );
            const string expected = "9 => 0: func(arg: 1)";
            Assert.Equal(expected, call.ToString());
        }
    }
}
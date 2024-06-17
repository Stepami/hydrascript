using HydraScript.Lib.BackEnd;
using Xunit;

namespace HydraScript.Tests.Unit.BackEnd;

public class FunctionInfoTests
{
    [Theory]
    [InlineData("func", null, "func")]
    [InlineData("func", "obj", "obj.func")]
    public void CallIdCorrectTest(string id, string methodOf, string expected) =>
        Assert.Equal(expected, new FunctionInfo(id, methodOf).ToString());
}
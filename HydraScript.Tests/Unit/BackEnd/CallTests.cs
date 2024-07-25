using HydraScript.Lib.BackEnd;
using HydraScript.Lib.BackEnd.Addresses;
using Xunit;

namespace HydraScript.Tests.Unit.BackEnd;

public class CallTests
{
    [Fact]
    public void ToStringCorrect()
    {
        var call = new Call(
            new Label("9"),
            new FunctionInfo("func"),
            [("arg", 1)]);
        const string expected = "9:\n\t => Start_func:\n\t: func(arg: 1)";
        Assert.Equal(expected, call.ToString());
    }
}
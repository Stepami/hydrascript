using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using Xunit;

namespace HydraScript.UnitTests.Unit.BackEnd;

public class CallTests
{
    [Fact]
    public void ToStringCorrect()
    {
        var call = new Call(
            new Label("9"),
            new FunctionInfo("func"));
        const string expected = "9:\n\t:  => Start_func:\n\t: func";
        Assert.Equal(expected, call.ToString());
    }
}
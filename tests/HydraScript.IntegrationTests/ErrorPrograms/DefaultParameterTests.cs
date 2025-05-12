using HydraScript.Infrastructure;

namespace HydraScript.IntegrationTests.ErrorPrograms;

public class DefaultParameterTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void DefaultParameter_PlacedBeforeNamed_HydraScriptError()
    {
        const string script = "function func(a = 1, b: boolean) { }";
        using var runner = fixture.GetRunner(new TestHostFixture.Options(InMemoryScript: script));
        var code = runner.Invoke();
        code.Should().Be(Executor.ExitCodes.HydraScriptError);
        fixture.LogMessages.Should()
            .Contain(x =>
                x.Contains("The argument b: boolean of function func is placed after default value argument"));
    }

    [Fact]
    public void DefaultParameter_AmbiguousInvocation_HydraScriptError()
    {
        const string script =
                """
                function f(a = 0){}
                function f(a = 0, b = 1) {}
                function f(a = 0, b = 1, c = 2) {}

                f()
                """
            ;
        using var runner = fixture.GetRunner(new TestHostFixture.Options(InMemoryScript: script));
        var code = runner.Invoke();
        code.Should().Be(Executor.ExitCodes.HydraScriptError);
        const string expectedMessage =
            "Candidates are:\n" +
            "function f(number)\n" +
            "function f(number, number)\n" +
            "function f(number, number, number)";
        fixture.LogMessages.Should().Contain(x => x.Contains(expectedMessage));
    }
}
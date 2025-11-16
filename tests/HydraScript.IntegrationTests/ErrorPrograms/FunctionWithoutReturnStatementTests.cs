using HydraScript.Infrastructure;

namespace HydraScript.IntegrationTests.ErrorPrograms;

public class FunctionWithoutReturnStatementTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void FunctionDeclaration_MissingReturn_HydraScriptError()
    {
        const string script =
            """
            function f(b: boolean) {
                if (b)
                    return 1
            }
            """;
        using var runner = fixture.GetRunner(new TestHostFixture.Options(InMemoryScript: script));
        var code = runner.Invoke();
        code.Should().Be(Executor.ExitCodes.HydraScriptError);
        fixture.LogMessages.Should()
            .Contain(x =>
                x.Contains("function with non-void return type must have a return statement"));
    }
}
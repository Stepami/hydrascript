using HydraScript.Infrastructure;

namespace HydraScript.IntegrationTests.ErrorPrograms;

public class VoidAssignmentTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void FunctionDeclared_VoidReturnAssigned_HydraScriptError()
    {
        const string script =
            """
                function func(b: boolean) {
                    if (b)
                        return
                    return
                }
                let x = func(true)
            """;
        using var runner = fixture.GetRunner(new TestHostFixture.Options(InMemoryScript: script));
        var code = runner.Invoke();
        code.Should().Be(Executor.ExitCodes.HydraScriptError);
        fixture.LogMessages.Should()
            .Contain(x => x.Contains("Cannot assign void"));
    }
}
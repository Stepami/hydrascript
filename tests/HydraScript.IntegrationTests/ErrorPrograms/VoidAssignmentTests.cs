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
        var runner = fixture.GetRunner(configureTestServices:
            services => services.SetupInMemoryScript(script));
        var code = runner.Invoke(fixture.InMemoryScript);
        code.Should().Be(Executor.ExitCodes.HydraScriptError);
        fixture.LogMessages.Should()
            .Contain(x => x.Contains("Cannot assign void"));
    }
}
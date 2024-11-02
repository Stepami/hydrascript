using System.CommandLine.Parsing;

namespace HydraScript.IntegrationTests.ErrorPrograms;

public class VoidAssignmentTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void FunctionDeclared_VoidReturnAssigned_ExitCodeHydraScriptError()
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
        code.Should().Be(ExitCodes.HydraScriptError);
    }
}
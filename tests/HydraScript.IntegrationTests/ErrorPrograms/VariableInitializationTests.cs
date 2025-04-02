using HydraScript.Infrastructure;

namespace HydraScript.IntegrationTests.ErrorPrograms;

public class VariableInitializationTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Theory, MemberData(nameof(VariableInitializationScripts))]
    public void VariableWithoutTypeDeclared_AccessedBeforeInitialization_HydraScriptError(string script)
    {
        using var runner = fixture.GetRunner(new TestHostFixture.Options(InMemoryScript: script));
        var code = runner.Invoke();
        code.Should().Be(Executor.ExitCodes.HydraScriptError);
        fixture.LogMessages.Should()
            .Contain(x => x.Contains("Cannot access 'x' before initialization"));
    }

    public static TheoryData<string> VariableInitializationScripts
    {
        get
        {
            const string variableWithoutTypeDeclared =
                """
                let x = f()
                function f() {
                    >>>x
                    return 5
                }
                """;
            const string typedVariableDeclared =
                """
                let x: number = f()
                function f() {
                    >>>x
                    return 5
                }
                """;
            return new TheoryData<string>([
                variableWithoutTypeDeclared,
                typedVariableDeclared]);
        }
    }
}
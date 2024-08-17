using System.CommandLine.Parsing;
using FluentAssertions;
using Xunit.Abstractions;

namespace HydraScript.IntegrationTests.ErrorPrograms;

public class VariableInitializationTests(
    TestHostFixture fixture,
    ITestOutputHelper testOutputHelper) : IClassFixture<TestHostFixture>
{
    [Theory, MemberData(nameof(VariableInitializationScripts))]
    public void VariableWithoutTypeDeclared_AccessedBeforeInitialization_ExitCodeHydraScriptError(string script)
    {
        var runner = fixture.GetRunner(
            testOutputHelper,
            configureTestServices: services => services.SetupInMemoryScript(script));
        var code = runner.Invoke(fixture.InMemoryScript);
        code.Should().Be(ExitCodes.HydraScriptError);
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
                    print(x as string)
                    return 5
                }
                """;
            const string typedVariableDeclared =
                """
                let x: number = f()
                function f() {
                    print(x as string)
                    return 5
                }
                """;
            return new TheoryData<string>([variableWithoutTypeDeclared, typedVariableDeclared]);
        }
    }
}
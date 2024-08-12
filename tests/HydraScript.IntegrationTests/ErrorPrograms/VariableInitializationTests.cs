using System.CommandLine.Parsing;
using FluentAssertions;
using Xunit.Abstractions;

namespace HydraScript.IntegrationTests.ErrorPrograms;

public class VariableInitializationTests(
    TestHostFixture fixture,
    ITestOutputHelper testOutputHelper) : IClassFixture<TestHostFixture>, IDisposable
{
    private readonly StringWriter _writer = new();

    [Fact]
    public void VariableWithoutTypeDeclared_AccessedBeforeInitialization_ExitCodeHydraScriptError()
    {
        const string script =
"""
let x = f()
function f() {
    print(x as string)
    return 5
}
""";
        var runner = fixture.GetRunner(
            testOutputHelper,
            _writer,
            configureTestServices: services => services.SetupInMemoryScript(script));
        var code = runner.Invoke(fixture.InMemoryScript);
        code.Should().Be(ExitCodes.HydraScriptError);
        var output = _writer.ToString().Trim();
        output.Should().Be("(3, 11)-(3, 12) Cannot access 'x' before initialization");
    }

    [Fact]
    public void TypedVariableDeclared_AccessedBeforeInitialization_ExitCodeHydraScriptError()
    {
        const string script =
"""
let x: number = f()
function f() {
    print(x as string)
    return 5
}
""";
        var runner = fixture.GetRunner(
            testOutputHelper,
            _writer,
            configureTestServices: services => services.SetupInMemoryScript(script));
        var code = runner.Invoke(fixture.InMemoryScript);
        code.Should().Be(ExitCodes.HydraScriptError);
        var output = _writer.ToString().Trim();
        output.Should().Be("(3, 11)-(3, 12) Cannot access 'x' before initialization");
    }

    public void Dispose()
    {
        _writer.Dispose();
        fixture.Dispose();
    }
}
using HydraScript.Infrastructure;

namespace HydraScript.IntegrationTests.ErrorPrograms;

public class NullAssignmentWhenUndefinedTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Theory, MemberData(nameof(NullAssignmentScripts))]
    public void NullAssignment_UndefinedDestinationOrReturnType_HydraScriptError(string script)
    {
        var runner = fixture.GetRunner(
            configureTestServices: services =>
                services.SetupInMemoryScript(script));
        var code = runner.Invoke(fixture.InMemoryScript);
        code.Should().Be(Executor.ExitCodes.HydraScriptError);
        fixture.LogMessages.Should()
            .Contain(x => x.Contains("Cannot assign 'null' when type is undefined"));
    }

    public static TheoryData<string> NullAssignmentScripts
    {
        get
        {
            const string lexicalDeclaration = "let x = null";
            const string objectLiteralProperty = "let y = {prop: null;}";
            const string functionReturn = "function f() { return null }";
            return new TheoryData<string>([
                lexicalDeclaration,
                objectLiteralProperty,
                functionReturn]);
        }
    }
}
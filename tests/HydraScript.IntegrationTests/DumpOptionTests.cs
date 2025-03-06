using System.CommandLine.Parsing;
using System.IO.Abstractions;
using NSubstitute;

namespace HydraScript.IntegrationTests;

public class DumpOptionTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void Invoke_DumpOptionPassed_FilesCreated()
    {
        var fileSystemMock = Substitute.For<IFileSystem>();
        var runner = fixture.GetRunner(
            configureTestServices: services =>
                services.SetupInMemoryScript(script: string.Empty, fileSystemMock));
        runner.Invoke(fixture.InMemoryScriptWithDump);

        fileSystemMock.File.Received(1)
            .WriteAllText(
                TestHostFixture.ScriptFileName + ".tokens",
                Arg.Any<string>());
        fileSystemMock.File.Received(1)
            .WriteAllText(
                "ast.dot",
                Arg.Any<string>());
        fileSystemMock.File.Received(1)
            .WriteAllLines(
                TestHostFixture.ScriptFileName + ".tac",
                Arg.Any<IEnumerable<string>>());
    }
}
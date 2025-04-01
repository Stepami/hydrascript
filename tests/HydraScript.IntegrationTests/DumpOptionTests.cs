using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace HydraScript.IntegrationTests;

public class DumpOptionTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void Invoke_DumpOptionPassed_FilesCreated()
    {
        var runner = fixture.GetRunner(new TestHostFixture.Options(Dump: true));
        runner.Invoke();

        var fileSystemMock = runner.ServiceProvider.GetRequiredService<IFileSystem>();
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
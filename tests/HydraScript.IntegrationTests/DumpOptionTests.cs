using System.IO.Abstractions;
using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace HydraScript.IntegrationTests;

public class DumpOptionTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void Invoke_DumpOptionPassed_FilesCreated()
    {
        using var runner = fixture.GetRunner(new TestHostFixture.Options(Dump: true, InMemoryScript: ">>>[]"));
        var outputWriter = runner.ServiceProvider.GetRequiredService<IConsole>();
        var fileSystemMock = runner.ServiceProvider.GetRequiredService<IFileSystem>();
        fileSystemMock.File
            .WhenForAnyArgs(x => x.WriteAllText(Arg.Any<string>(), Arg.Any<string>()))
            .Do(x =>
            {
                outputWriter.WriteLine(x.ArgAt<string>(0));
                outputWriter.WriteLine(x.ArgAt<string>(1));
            });

        runner.Invoke();
        fileSystemMock.File.Received(1)
            .WriteAllText(
                Arg.Is<string>(s => s.EndsWith(TestHostFixture.ScriptFileName + ".tokens")),
                Arg.Any<string>());
        fileSystemMock.File.Received(1)
            .WriteAllText(
                Arg.Is<string>(s => s.EndsWith(TestHostFixture.ScriptFileName + ".dot")),
                Arg.Any<string>());
        fileSystemMock.File.Received(1)
            .WriteAllText(
                Arg.Is<string>(s => s.EndsWith(TestHostFixture.ScriptFileName + ".tac")),
                Arg.Any<string>());
    }
}
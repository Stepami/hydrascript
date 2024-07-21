using System.IO.Abstractions;
using Microsoft.Extensions.Options;

namespace HydraScript.Services.SourceCode.Impl;

internal class SourceCodeProvider : ISourceCodeProvider
{
    private readonly IFileSystem _fileSystem;
    private readonly CommandLineSettings _commandLineSettings;

    public SourceCodeProvider(
        IFileSystem fileSystem,
        IOptions<CommandLineSettings> commandLineSettings)
    {
        _fileSystem = fileSystem;
        _commandLineSettings = commandLineSettings.Value;
    }

    public string GetText()
    {
        var inputFilePath = _commandLineSettings.InputFilePath;
        return _fileSystem.File.ReadAllText(inputFilePath);
    }
}
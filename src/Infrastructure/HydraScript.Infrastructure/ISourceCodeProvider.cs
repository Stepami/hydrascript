using System.IO.Abstractions;
using Microsoft.Extensions.Options;

namespace HydraScript.Infrastructure;

public interface ISourceCodeProvider
{
    string GetText();
}

internal class SourceCodeProvider(
    IFileSystem fileSystem,
    IOptions<InputFile> inputFile) : ISourceCodeProvider
{
    private readonly InputFile _inputFile = inputFile.Value;

    public string GetText()
    {
        var inputFilePath = _inputFile.Path;
        return fileSystem.File.ReadAllText(inputFilePath);
    }
}
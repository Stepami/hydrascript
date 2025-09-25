using System.IO.Abstractions;
using Microsoft.Extensions.Options;

namespace HydraScript.Infrastructure;

public interface ISourceCodeProvider
{
    string GetText();
}

internal class SourceCodeProvider(
    IFileSystem fileSystem,
    IOptions<FileInfo> inputFile) : ISourceCodeProvider
{
    public string GetText()
    {
        var inputFilePath = inputFile.Value.FullName;
        return fileSystem.File.ReadAllText(inputFilePath);
    }
}
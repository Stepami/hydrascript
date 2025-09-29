using System.IO.Abstractions;
using Cysharp.Text;
using Microsoft.Extensions.Options;

namespace HydraScript.Infrastructure.Dumping;

public interface IDumpingService
{
    void Dump(string? contents, string fileExtension);
}

internal sealed class DumpingService(
    IFileSystem fileSystem,
    IOptions<FileInfo> fileInfo) : IDumpingService
{
    public void Dump(string? contents, string fileExtension)
    {
        var fileNameWithExtension = fileInfo.Value.Name;
        var originalFileExtension = fileInfo.Value.Extension;
        var fileName = fileNameWithExtension.Replace(originalFileExtension, string.Empty);
        var path = Path.Combine(
            fileInfo.Value.DirectoryName ?? string.Empty,
            ZString.Concat(fileName, ".", fileExtension));
        fileSystem.File.WriteAllText(path, contents);
    }
}
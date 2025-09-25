using Microsoft.Extensions.Options;

namespace HydraScript.Benchmarks;

internal sealed class UpdatableFileOptions(FileInfo fileInfo) : IOptions<FileInfo>
{
    public FileInfo Value { get; private set; } = fileInfo;

    public void Update(FileInfo fileInfo) => Value = fileInfo;
}
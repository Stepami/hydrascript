namespace HydraScript.Infrastructure.Dumping;

public interface IDumpingService
{
    void Dump(string content, string fileExtension);
}

internal sealed class DumpingService : IDumpingService
{
    public void Dump(string content, string fileExtension)
    {
        throw new NotImplementedException();
    }
}
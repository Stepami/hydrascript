namespace HydraScript.Domain.BackEnd;

public interface IOutputWriter
{
    public void WriteLine(object? obj);

    public void WriteError(Exception e, string message);
}
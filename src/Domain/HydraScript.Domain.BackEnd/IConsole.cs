namespace HydraScript.Domain.BackEnd;

public interface IConsole
{
    public void WriteLine(object? obj);

    public void WriteError(Exception e, string message);

    public string ReadLine();
}
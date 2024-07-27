namespace HydraScript.Domain.IR;

public interface ISymbol
{
    public string Id { get; }
    public Type Type { get; }
}
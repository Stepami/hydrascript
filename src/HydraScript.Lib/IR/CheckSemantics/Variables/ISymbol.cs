namespace HydraScript.Lib.IR.CheckSemantics.Variables;

public interface ISymbol
{
    public string Id { get; }
    public Type Type { get; }
}
namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

public abstract class Symbol
{
    public abstract string Id { get; }
    public abstract Type Type { get; }
    public SymbolState State { get; protected set; } = SymbolState.Declared;
}

public enum SymbolState
{
    Declared,
    Initialized
}
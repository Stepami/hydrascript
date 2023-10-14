namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

public abstract class Symbol
{
    public abstract string Id { get; }
    public abstract Type Type { get; }
    public SymbolState State { get; protected set; } = SymbolState.Declared;

    public void SetInitialized()
    {
        if (State is not SymbolState.Declared)
            throw new InvalidOperationException(
                message: "Only declared symbols can be initialized");

        State = SymbolState.Initialized;
    }
}

public enum SymbolState
{
    Declared,
    Initialized
}
namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

public abstract class Symbol
{
    public abstract string Id { get; }
    public abstract Type Type { get; }
}
namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

public class TypeSymbol : Symbol
{
    public TypeSymbol(Type type, string id = null) :
        base(id ?? type.ToString(), type) { }

    public override string ToString() =>
        $"type {Id} = {Type}";
}
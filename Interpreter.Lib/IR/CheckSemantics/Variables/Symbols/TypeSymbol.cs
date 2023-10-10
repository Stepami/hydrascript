namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

public class TypeSymbol : Symbol
{
    public override string Id { get; }
    public override Type Type { get; }
    
    public TypeSymbol(Type type, string id = null)
    {
        Id = id ?? type.ToString();
        Type = type;
    }

    public override bool Equals(object obj)
    {
        if (obj is TypeSymbol typeSymbol)
        {
            return Id == typeSymbol.Id &&
                   Type.Equals(typeSymbol.Type);
        }

        return false;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, Type);

    public override string ToString() =>
        $"type {Id} = {Type}";
}
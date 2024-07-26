namespace HydraScript.Lib.BackEnd.Impl.Values;

public class Constant(object? value, string representation) : IValue
{
    private readonly object? _value = value;

    public Constant(object value) :
        this(value, representation: value.ToString()!)
    {
    }

    public object? Get(Frame? frame) => _value;

    public override string ToString() => representation;

    public bool Equals(IValue? other) =>
        other is Constant that &&
        Equals(_value, that._value);
}
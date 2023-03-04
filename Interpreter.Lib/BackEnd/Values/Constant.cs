namespace Interpreter.Lib.BackEnd.Values;

public class Constant : IValue
{
    private readonly object _value;
    private readonly string _representation;

    public Constant(object value, string representation)
    {
        _value = value;
        _representation = representation;
    }

    public Constant(object value) :
        this(value, value.ToString()) { }

    public object Get(Frame frame) => _value;

    public override string ToString() => _representation;

    public bool Equals(IValue other)
    {
        if (other is Constant that)
        {
            return Equals(_value, that._value);
        }

        return false;
    }
}
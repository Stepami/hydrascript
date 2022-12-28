namespace Interpreter.Lib.BackEnd.Addresses;

public class Label : IAddress
{
    private readonly string _name;

    public Label(string name) =>
        _name = name;
    
    public IAddress Next { get; set; }

    public bool IsLabel() => true;

    public bool Equals(IAddress other)
    {
        if (other is Label label)
            return _name == label._name;

        return false;
    }

    public override int GetHashCode() =>
        _name.GetHashCode();

    public override string ToString() => _name;
}
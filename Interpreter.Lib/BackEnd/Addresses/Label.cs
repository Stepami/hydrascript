namespace Interpreter.Lib.BackEnd.Addresses;

public class Label : IAddress
{
    public string Name { get; }

    public Label(string name) =>
        Name = name;
    
    public IAddress Next { get; set; }

    public bool Equals(IAddress other)
    {
        if (other is Label label)
            return Name == label.Name;

        return false;
    }

    public override int GetHashCode() =>
        Name.GetHashCode();

    public override string ToString() =>
        $"{Name}:\n\t";
}
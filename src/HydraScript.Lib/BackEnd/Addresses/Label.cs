namespace HydraScript.Lib.BackEnd.Addresses;

public class Label(string name) : IAddress
{
    public string Name { get; } = name;

    public IAddress Next { get; set; } = default!;

    public bool Equals(IAddress? other) =>
        other is Label label &&
        Name == label.Name;

    public override int GetHashCode() =>
        Name.GetHashCode();

    public override string ToString() =>
        $"{Name}:\n\t";
}
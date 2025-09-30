namespace HydraScript.Domain.BackEnd;

public interface IAddress : IEquatable<IAddress>
{
    public IAddress? Next { get; set; }

    public string Name { get; }
}
namespace HydraScript.Domain.BackEnd;

public interface IAddress : IEquatable<IAddress>
{
    IAddress Next { get; set; }
}
namespace HydraScript.Lib.BackEnd;

public interface IAddress : IEquatable<IAddress>
{
    IAddress Next { get; set; }
}
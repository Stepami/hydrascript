namespace Interpreter.Lib.BackEnd.Addresses;

public interface IAddress : IEquatable<IAddress>
{
    IAddress Next { get; set; }
}
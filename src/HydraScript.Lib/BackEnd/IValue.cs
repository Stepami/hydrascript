namespace HydraScript.Lib.BackEnd;

public interface IValue : IEquatable<IValue>
{
    object? Get(Frame? frame);
}
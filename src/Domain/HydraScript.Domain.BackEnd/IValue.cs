namespace HydraScript.Domain.BackEnd;

public interface IValue : IEquatable<IValue>
{
    object? Get();
}
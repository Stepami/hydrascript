namespace HydraScript.Lib.BackEnd.Values;

public interface IValue : IEquatable<IValue>
{
    object Get(Frame frame);
}
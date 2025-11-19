namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public abstract class Instruction : IExecutableInstruction
{
    public IAddress Address
    {
        get;
        set
        {
            OnSetOfAddress(value);
            field = value;
        }
    } = null!;

    protected virtual void OnSetOfAddress(IAddress address) { }

    public abstract IAddress? Execute(IExecuteParams executeParams);

    protected abstract string ToStringInternal();

    public override string ToString() =>
        $"{Address}{ToStringInternal()}";
}
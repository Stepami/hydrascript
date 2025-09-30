namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public abstract class Instruction : IExecutableInstruction
{
    private IAddress _address = null!;

    public IAddress Address
    {
        get => _address;
        set
        {
            OnSetOfAddress(value);
            _address = value;
        }
    }
    
    protected virtual void OnSetOfAddress(IAddress address) { }

    public abstract IAddress? Execute(IExecuteParams executeParams);

    protected abstract string ToStringInternal();

    public override string ToString() =>
        $"{Address}{ToStringInternal()}";
}
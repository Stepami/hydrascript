namespace HydraScript.Lib.BackEnd.Instructions;

public abstract class Instruction
{
    private IAddress _address = default!;

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

    public abstract IAddress Execute(VirtualMachine vm);
    
    public virtual bool End => false;

    protected abstract string ToStringInternal();

    public override string ToString() =>
        $"{Address}{ToStringInternal()}";
}
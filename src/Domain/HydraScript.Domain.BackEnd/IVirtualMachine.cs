namespace HydraScript.Domain.BackEnd;

public interface IVirtualMachine
{
    public IExecuteParams ExecuteParams { get; }
    public void Run(AddressedInstructions instructions);
}
namespace HydraScript.Domain.BackEnd;

public interface IExecutableInstruction
{
    public IAddress Address { get; set; }
    public IAddress? Execute(IExecuteParams executeParams);
}
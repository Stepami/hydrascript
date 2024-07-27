namespace HydraScript.Lib.BackEnd;

public interface IExecutableInstruction
{
    public IAddress Address { get; set; }
    public IAddress Execute(IExecuteParams executeParams);
    public bool End { get; }
}
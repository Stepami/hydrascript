namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;

public interface IReadFromComplexData
{
    Simple ToAssignment(IValue value);
    
    IExecutableInstruction ToInstruction();
}
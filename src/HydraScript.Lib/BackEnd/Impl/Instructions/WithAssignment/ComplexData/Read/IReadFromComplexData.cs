namespace HydraScript.Lib.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;

public interface IReadFromComplexData
{
    Simple ToAssignment(IValue value);
    
    Instruction ToInstruction();
}
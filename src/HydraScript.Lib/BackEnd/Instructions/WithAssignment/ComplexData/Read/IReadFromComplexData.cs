namespace HydraScript.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Read;

public interface IReadFromComplexData
{
    Simple ToAssignment(IValue value);
    
    Instruction ToInstruction();
}
namespace Interpreter.Lib.BackEnd;

public interface IAddress
{
    IAddress Next();
    
    void Unlink();
}
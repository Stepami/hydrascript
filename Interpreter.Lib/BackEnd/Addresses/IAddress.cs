namespace Interpreter.Lib.BackEnd.Addresses;

public interface IAddress
{
    IAddress Next();
    
    void Unlink();
}
namespace Interpreter.Lib.VM
{
    public interface IValue
    {
        object Get(Frame frame);
    }
}
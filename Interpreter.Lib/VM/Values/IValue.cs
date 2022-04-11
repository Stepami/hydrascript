namespace Interpreter.Lib.VM.Values
{
    public interface IValue
    {
        object Get(Frame frame);
    }
}
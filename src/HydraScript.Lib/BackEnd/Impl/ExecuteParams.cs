namespace HydraScript.Lib.BackEnd.Impl;

public class ExecuteParams(TextWriter textWriter) : IExecuteParams
{
    public Stack<Call> CallStack { get; } = new();
    public Stack<Frame> Frames { get; } = new();
    public Queue<object?> Arguments { get; } = new();
    public TextWriter Writer { get; } = textWriter;
}
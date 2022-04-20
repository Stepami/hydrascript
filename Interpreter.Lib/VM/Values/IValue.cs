using System;

namespace Interpreter.Lib.VM.Values
{
    public interface IValue : IEquatable<IValue>
    {
        object Get(Frame frame);
    }
}
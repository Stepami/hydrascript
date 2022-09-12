using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.VM
{
    public class VirtualMachine
    {
        public Stack<Call> CallStack { get; } = new();
        
        public Stack<Frame> Frames { get; } = new();
        
        public Stack<(string Id, object Value)> Arguments { get; } = new();

        public void Run(List<Instruction> instructions)
        {
            Frames.Push(new Frame());

            var address = 0;
            while (!instructions[address].End())
            {
                var instruction = instructions[address];
                var jump = instruction.Execute(this);
                address = jump;
            }
        }
    }
}
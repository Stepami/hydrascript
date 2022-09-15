using System;
using System.Collections.Generic;
using System.IO;
using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.BackEnd.VM
{
    public record VirtualMachine(
        Stack<Call> CallStack, Stack<Frame> Frames,
        Stack<(string Id, object Value)> Arguments,
        TextWriter Writer
    )
    {
        public VirtualMachine() :
            this(new(), new(), new(), Console.Out) { }

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
using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.VM
{
    public class VirtualMachine
    {
        private readonly ControlFlowGraph cfg;
        private readonly Stack<Call> callStack = new();
        private readonly Stack<Frame> frames = new();
        private readonly Stack<(string Id, object Value)> arguments = new();

        public VirtualMachine(ControlFlowGraph cfg)
        {
            this.cfg = cfg;
        }

        public void Run()
        {
            var block = cfg.Entry;
            var instructions = new Stack<Instruction>(block.Reverse());
            
            frames.Push(new Frame());

            while (!instructions.Peek().End())
            {
                var instruction = instructions.Pop();
                var jump = instruction.Execute(callStack, frames, arguments);

                if (instructions.Any())
                {
                    continue;
                }

                block = cfg.NextBlock(block, jump);
                instructions = new Stack<Instruction>(block.Reverse());
            }

            instructions.Pop().Execute(callStack, frames, arguments);
        }
    }
}
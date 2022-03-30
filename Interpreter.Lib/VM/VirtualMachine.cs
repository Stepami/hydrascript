using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.VM
{
    public class VirtualMachine
    {
        private readonly ControlFlowGraph _cfg;
        private readonly Stack<Call> _callStack = new();
        private readonly Stack<Frame> _frames = new();
        private readonly Stack<(string Id, object Value)> _arguments = new();

        public VirtualMachine(ControlFlowGraph cfg)
        {
            _cfg = cfg;
        }

        public void Run()
        {
            var block = _cfg.Entry;
            var instructions = new Stack<Instruction>(block.Reverse());
            
            _frames.Push(new Frame());

            while (!instructions.Peek().End())
            {
                var instruction = instructions.Pop();
                var jump = instruction.Execute(_callStack, _frames, _arguments);

                if (instructions.Any())
                {
                    continue;
                }

                block = _cfg.NextBlock(block, jump);
                instructions = new Stack<Instruction>(block.Reverse());
            }

            instructions.Pop().Execute(_callStack, _frames, _arguments);
        }
    }
}
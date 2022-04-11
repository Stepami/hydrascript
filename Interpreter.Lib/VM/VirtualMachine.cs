using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.VM
{
    public class VirtualMachine
    {
        private readonly ControlFlowGraph _cfg;
        
        public Stack<Call> CallStack { get; } = new();
        
        public Stack<Frame> Frames { get; } = new();
        
        public Stack<(string Id, object Value)> Arguments { get; } = new();

        public VirtualMachine(ControlFlowGraph cfg)
        {
            _cfg = cfg;
        }

        public void Run()
        {
            var block = _cfg.Entry;
            var instructions = new Stack<Instruction>(block.Reverse());
            
            Frames.Push(new Frame());

            while (!instructions.Peek().End())
            {
                var instruction = instructions.Pop();
                var jump = instruction.Execute(this);

                if (instructions.Any())
                {
                    continue;
                }

                block = _cfg.NextBlock(block, jump);
                instructions = new Stack<Instruction>(block.Reverse());
            }

            instructions.Pop().Execute(this);
        }
    }
}
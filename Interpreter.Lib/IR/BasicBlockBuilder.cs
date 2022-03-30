using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.IR
{
    public class BasicBlockBuilder
    {
        private readonly List<Instruction> _instructions;

        public BasicBlockBuilder(List<Instruction> instructions)
        {
            _instructions = instructions;
        }

        public List<BasicBlock> GetBasicBlocks()
        {
            _instructions[0].Leader = true;

            _instructions
                .Where(i => i.Branch())
                .Select(i => new
                {
                    Jump = i.Jump(),
                    Next = i.Number + 1
                })
                .ToList()
                .ForEach(obj =>
                {
                    _instructions[obj.Jump].Leader = true;
                    _instructions[obj.Next].Leader = true;
                });

            var basicBlocks = new Stack<BasicBlock>();
            var instructionsInBlock = new List<Instruction>();
            foreach (var instruction in _instructions.AsEnumerable().Reverse())
            {
                instructionsInBlock.Add(instruction);
                if (instruction.Leader)
                {
                    basicBlocks.Push(new BasicBlock(instructionsInBlock));
                    instructionsInBlock.Clear();
                }
            }

            return basicBlocks.ToList();
        }
    }
}
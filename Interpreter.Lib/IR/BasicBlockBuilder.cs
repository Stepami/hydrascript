using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.IR
{
    public class BasicBlockBuilder
    {
        private readonly List<Instruction> instructions;

        public BasicBlockBuilder(List<Instruction> instructions)
        {
            this.instructions = instructions;
        }

        public List<BasicBlock> GetBasicBlocks()
        {
            instructions[0].Leader = true;

            instructions
                .Where(i => i.Branch())
                .Select(i => new
                {
                    Jump = i.Jump(),
                    Next = i.Number + 1
                })
                .ToList()
                .ForEach(obj =>
                {
                    instructions[obj.Jump].Leader = true;
                    instructions[obj.Next].Leader = true;
                });

            var basicBlocks = new Stack<BasicBlock>();
            var instructionsInBlock = new List<Instruction>();
            foreach (var instruction in instructions.AsEnumerable().Reverse())
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
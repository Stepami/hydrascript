using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.IR.Optimizers;

namespace Interpreter.Lib.IR
{
    public class ControlFlowGraph
    {
        private readonly Dictionary<BasicBlock, List<BasicBlock>> _adjacencyList = new();

        public ControlFlowGraph(List<BasicBlock> basicBlocks)
        {
            basicBlocks.ForEach(basicBlock =>
                _adjacencyList[basicBlock] = basicBlocks
                    .Where(bb => basicBlock.Out().Contains(bb.In()))
                    .ToList()
            );
        }

        public BasicBlock Entry => _adjacencyList.Keys.Min();

        public BasicBlock NextBlock(BasicBlock current, int jump) =>
            _adjacencyList[current]
                .FirstOrDefault(bb => bb.In() == jump);

        public void OptimizeInstructions(params Func<Instruction, IOptimizer<Instruction>>[] generators)
        {
            foreach (var bb in _adjacencyList.Keys)
            {
                foreach (var instruction in bb)
                {
                    foreach (var generator in generators)
                    {
                        var optimizer = generator(null);
                        if (optimizer.CanOptimize(instruction))
                        {
                            optimizer = generator(instruction);
                            optimizer.Optimize();
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder("digraph cfg {\n");
            foreach (var basicBlock in _adjacencyList.Keys)
            {
                result.Append($"\t{basicBlock}\n");
                foreach (var bb in _adjacencyList[basicBlock])
                {
                    result.Append($"\t{basicBlock.GetHashCode()}->{bb.GetHashCode()}\n");
                }
            }

            return result.Append('}').ToString();
        }
    }
}
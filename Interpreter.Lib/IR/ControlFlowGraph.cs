using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Lib.IR
{
    public class ControlFlowGraph
    {
        private readonly Dictionary<BasicBlock, List<BasicBlock>> adjacencyList = new();

        public ControlFlowGraph(List<BasicBlock> basicBlocks)
        {
            basicBlocks.ForEach(basicBlock =>
                adjacencyList[basicBlock] = basicBlocks
                    .Where(bb => basicBlock.Out().Contains(bb.In()))
                    .ToList()
            );
        }

        public BasicBlock Entry => adjacencyList.Keys.Min();

        public BasicBlock NextBlock(BasicBlock current, int jump) =>
            adjacencyList[current]
                .FirstOrDefault(bb => bb.In() == jump);

        public override string ToString()
        {
            var result = new StringBuilder("digraph cfg {\n");
            foreach (var basicBlock in adjacencyList.Keys)
            {
                result.Append($"\t{basicBlock}\n");
                foreach (var bb in adjacencyList[basicBlock])
                {
                    result.Append($"\t{basicBlock.GetHashCode()}->{bb.GetHashCode()}\n");
                }
            }

            return result.Append('}').ToString();
        }
    }
}
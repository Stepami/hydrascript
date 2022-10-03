using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Nodes;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl
{
    public class AbstractSyntaxTree : IAbstractSyntaxTree
    {
        private readonly AbstractSyntaxTreeNode _root;
        private readonly InstructionProvider _instructionProvider;

        public AbstractSyntaxTree(AbstractSyntaxTreeNode root)
        {
            _root = root;
            _instructionProvider = new();
        }

        private void Check() =>
            GetAllNodes().ToList().ForEach(node => node.SemanticCheck());

        private IEnumerable<AbstractSyntaxTreeNode> GetAllNodes() =>
            _root.GetAllNodes();

        public List<Instruction> GetInstructions()
        {
            //Check();

            var newResult = _root.Accept(_instructionProvider);
            var start = 0;
            var result = new List<Instruction>();
            foreach (var node in _root)
            {
                var instructions = node.ToInstructions(start);
                result.AddRange(instructions);
                start += instructions.Count;
            }

            result.Sort();
            result.Add(new Halt(result.Count));

            var calls = result.OfType<CallFunction>().GroupBy(i => i.Jump());
            foreach (var call in calls)
            {
                var returns = result.OfType<Return>()
                    .Where(r => r.FunctionStart == call.Key);
                foreach (var ret in returns)
                {
                    foreach (var caller in call)
                    {
                        ret.AddCaller(caller.Number + 1);
                    }
                }
            }
            return result;
        }

        public override string ToString()
        {
            var tree = new StringBuilder("digraph ast {\n");
            _root.GetAllNodes().ForEach(node =>
            {
                tree.Append('\t').Append(node).Append('\n');
                node.ToList().ForEach(child => tree.Append($"\t{node.GetHashCode()}->{child.GetHashCode()}\n"));
            });
            return tree.Append("}\n").ToString();
        }
    }
}
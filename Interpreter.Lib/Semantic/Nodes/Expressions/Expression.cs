using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public abstract class Expression : AbstractSyntaxTreeNode
    {
        protected Expression()
        {
            CanEvaluate = true;
        }

        public bool Primary() => !this.Any();

        public abstract List<Instruction> ToInstructions(int start, string temp);
    }
}
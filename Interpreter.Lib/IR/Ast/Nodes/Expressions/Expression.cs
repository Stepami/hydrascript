using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions
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
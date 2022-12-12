using Interpreter.Lib.IR.CheckSemantics.Exceptions;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements
{
    public abstract class InsideLoopStatement : Statement
    {
        protected InsideLoopStatement()
        {
            CanEvaluate = true;
        }
        
        internal override Type NodeCheck()
        {
            if (!ChildOf<WhileStatement>())
            {
                throw new OutsideOfLoop(Segment, NodeRepresentation());
            }
            return null;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield break;
        }
    }
}
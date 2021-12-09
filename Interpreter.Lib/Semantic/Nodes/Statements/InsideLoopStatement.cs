using System.Collections.Generic;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Nodes.Statements
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
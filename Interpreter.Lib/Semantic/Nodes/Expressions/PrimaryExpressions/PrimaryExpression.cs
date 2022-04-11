using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions
{
    public abstract class PrimaryExpression : Expression
    {
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield break;
        }

        public abstract IValue ToValue();
        
        public override List<Instruction> ToInstructions(int start, string temp) =>
            new()
            {
                new Simple(temp, (null, ToValue()), "", start)
            };
    }
}
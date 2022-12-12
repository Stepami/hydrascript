using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions
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
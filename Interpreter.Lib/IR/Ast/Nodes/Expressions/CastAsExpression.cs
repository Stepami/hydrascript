using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions
{
    public class CastAsExpression : Expression
    {
        private readonly Expression _expression;
        private readonly Type _cast;

        public CastAsExpression(Expression expression, Type cast)
        {
            _expression = expression;
            _expression.Parent = this;

            _cast = cast;
        }

        internal override Type NodeCheck() => 
            TypeUtils.JavaScriptTypes.String;

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _expression;
        }

        protected override string NodeRepresentation() => $"as {_cast}";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            var instructions = new List<Instruction>();
            var castNumber = start;

            if (!_expression.Primary())
            {
                instructions.AddRange(_expression.ToInstructions(start, "_t"));
                castNumber = instructions.Last().Number + 1;
            }

            instructions.Add(new AsString(
                "_t" + castNumber,
                _expression.Primary()
                    ? ((PrimaryExpression) _expression).ToValue()
                    : new Name(instructions.OfType<Simple>().Last().Left),
                castNumber
            ));

            return instructions;
        }
    }
}
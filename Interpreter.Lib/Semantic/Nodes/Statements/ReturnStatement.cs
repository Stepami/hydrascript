using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Nodes.Expressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM.Values;

namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public class ReturnStatement : Statement
    {
        private readonly Expression _expression;

        public ReturnStatement(Expression expression = null)
        {
            _expression = expression;
            CanEvaluate = true;
            if (expression != null)
            {
                _expression.Parent = this;
            }
        }

        internal override Type NodeCheck()
        {
            if (!ChildOf<FunctionDeclaration>())
            {
                throw new ReturnOutsideFunction(Segment);
            }

            return _expression?.NodeCheck() ?? TypeUtils.JavaScriptTypes.Void;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            if (_expression == null)
            {
                yield break;
            }

            yield return _expression;
        }

        protected override string NodeRepresentation() => "return";

        private FunctionSymbol GetCallee()
        {
            var parent = Parent;
            while (parent != null)
            {
                if (parent is FunctionDeclaration declaration)
                {
                    return declaration.GetSymbol();
                }

                parent = parent.Parent;
            }

            return null;
        }

        public override List<Instruction> ToInstructions(int start)
        {
            var instructions = new List<Instruction>();
            if (_expression == null)
            {
                instructions.Add(new Return(GetCallee().CallInfo.Location, start));
            }
            else
            {
                if (_expression.Primary())
                {
                    instructions.Add(new Return(
                        GetCallee().CallInfo.Location, start, ((PrimaryExpression) _expression).ToValue())
                    );
                }
                else
                {
                    var eInstructions = _expression.ToInstructions(start, "_t");
                    var last = eInstructions.OfType<Simple>().Last();
                    instructions.AddRange(eInstructions);
                    instructions.Add(new Return(
                        GetCallee().CallInfo.Location, last.Number + 1, new Name(last.Left)
                    ));
                }
            }

            return instructions;
        }
    }
}
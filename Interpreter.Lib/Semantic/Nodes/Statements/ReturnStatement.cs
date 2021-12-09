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
using Interpreter.Lib.VM;

namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public class ReturnStatement : Statement
    {
        private readonly Expression expression;

        public ReturnStatement(Expression expression = null)
        {
            this.expression = expression;
            CanEvaluate = true;
            if (expression != null)
            {
                this.expression.Parent = this;
            }
        }

        internal override Type NodeCheck()
        {
            if (!ChildOf<FunctionDeclaration>())
            {
                throw new ReturnOutsideFunction(Segment);
            }

            return expression?.NodeCheck() ?? TypeUtils.JavaScriptTypes.Void;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            if (expression == null)
            {
                yield break;
            }

            yield return expression;
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
            if (expression == null)
            {
                instructions.Add(new ReturnInstruction(GetCallee().CallInfo.Location, start));
            }
            else
            {
                if (expression.Primary())
                {
                    instructions.Add(new ReturnInstruction(
                        GetCallee().CallInfo.Location, start, ((PrimaryExpression) expression).ToValue())
                    );
                }
                else
                {
                    var eInstructions = expression.ToInstructions(start, "_t");
                    var last = eInstructions.OfType<ThreeAddressCodeInstruction>().Last();
                    instructions.AddRange(eInstructions);
                    instructions.Add(new ReturnInstruction(
                        GetCallee().CallInfo.Location, last.Number + 1, new Name(last.Left)
                    ));
                }
            }

            return instructions;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public class WhileStatement : Statement
    {
        private readonly Expression condition;
        private readonly Statement statement;

        public WhileStatement(Expression condition, Statement statement)
        {
            this.condition = condition;
            this.condition.Parent = this;

            this.statement = statement;
            this.statement.Parent = this;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return condition;
            yield return statement;
        }

        protected override string NodeRepresentation() => "while";

        public override List<Instruction> ToInstructions(int start)
        {
            var instructions = new List<Instruction>();
            IValue ifNotTest;
            if (!condition.Primary())
            {
                var conditionInstructions = condition.ToInstructions(start, "_t");
                ifNotTest = new Name(conditionInstructions.OfType<ThreeAddressCodeInstruction>().Last().Left);
                instructions.AddRange(conditionInstructions);
            }
            else
            {
                ifNotTest = ((PrimaryExpression) condition).ToValue();
            }

            var cOffset = start + instructions.Count + 1;
            var loopBody = statement.ToInstructions(cOffset);
            if (loopBody.Any())
            {
                instructions.Add(new IfNotGotoInstruction(ifNotTest, loopBody.Last().Number + 2, cOffset - 1));
                instructions.AddRange(loopBody);
                instructions.Add(new GotoInstruction(start, loopBody.Last().Number + 1));

                loopBody
                    .OfType<GotoInstruction>()
                    .Where(g => g.Jump() < 0)
                    .ToList()
                    .ForEach(j =>
                    {
                        if (j.Jump() == -1)
                        {
                            j.SetJump(loopBody.Last().Number + 2);
                        }
                        else if (j.Jump() == -2)
                        {
                            j.SetJump(start);
                        }
                    });
            }
            else
            {
                instructions.Clear();
            }

            return instructions;
        }
    }
}
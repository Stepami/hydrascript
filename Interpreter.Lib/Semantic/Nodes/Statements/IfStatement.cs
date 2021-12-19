using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public class IfStatement : Statement
    {
        private readonly Expression test;
        private readonly Statement then;
        private readonly Statement @else;

        public IfStatement(Expression test, Statement then, Statement @else = null)
        {
            this.test = test;
            this.test.Parent = this;

            this.then = then;
            this.then.Parent = this;

            if (@else != null)
            {
                this.@else = @else;
                this.@else.Parent = this;
            }

            CanEvaluate = true;
        }

        public bool HasReturnStatement()
        {
            var thenResult = then is ReturnStatement;
            if (!thenResult)
            {
                if (then is BlockStatement block)
                {
                    thenResult = block.HasReturnStatement();
                }
            }

            var elseResult = @else == null || @else is ReturnStatement;
            if (!elseResult)
            {
                if (@else is BlockStatement block)
                {
                    elseResult = block.HasReturnStatement();
                }
            }

            return thenResult && elseResult;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return test;
            yield return then;
            if (@else != null)
            {
                yield return @else;
            }
        }

        internal override Type NodeCheck()
        {
            var testType = test.NodeCheck();
            if (!testType.Equals(TypeUtils.JavaScriptTypes.Boolean))
            {
                throw new NotBooleanTestExpression(Segment, testType);
            }

            return testType;
        }

        protected override string NodeRepresentation() => "if";

        public override List<Instruction> ToInstructions(int start)
        {
            var instructions = new List<Instruction>();
            if (then.Any() && (@else == null || @else.Any()))
            {
                IValue ifNotTest;
                if (!test.Primary())
                {
                    var testInstructions = test.ToInstructions(start, "_t");
                    ifNotTest = new Name(testInstructions.OfType<ThreeAddressCodeInstruction>().Last().Left);
                    instructions.AddRange(testInstructions);
                }
                else
                {
                    ifNotTest = ((PrimaryExpression) test).ToValue();
                }

                var tOffset = start + instructions.Count + 1;
                var thenInstructions = then.ToInstructions(tOffset);

                var eOffset = thenInstructions.Any()
                    ? thenInstructions.Last().Number + 2
                    : tOffset + 1;
                var elseInstructions = @else?.ToInstructions(eOffset);

                instructions.Add(
                    new IfNotGotoInstruction(
                        ifNotTest, elseInstructions?.First().Number ?? eOffset - 1, tOffset - 1
                    )
                );

                instructions.AddRange(thenInstructions);

                if (elseInstructions != null)
                {
                    instructions.Add(
                        new GotoInstruction(
                            elseInstructions.Last().Number + 1,
                            eOffset - 1
                        )
                    );
                    instructions.AddRange(elseInstructions);
                }
            }

            return instructions;
        }
    }
}
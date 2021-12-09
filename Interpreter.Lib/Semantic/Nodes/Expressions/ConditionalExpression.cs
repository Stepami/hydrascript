using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class ConditionalExpression : Expression
    {
        private readonly Expression test, consequent, alternate;

        public ConditionalExpression(Expression test, Expression consequent, Expression alternate)
        {
            this.test = test;
            this.consequent = consequent;
            this.alternate = alternate;
        }

        internal override Type NodeCheck()
        {
            var tType = test.NodeCheck();

            if (tType.Equals(TypeUtils.JavaScriptTypes.Boolean))
            {
                var cType = consequent.NodeCheck();
                var aType = alternate.NodeCheck();
                if (cType.Equals(aType))
                {
                    return cType;
                }

                throw new WrongConditionalTypes(consequent.Segment, cType, alternate.Segment, aType);
            }

            throw new NotBooleanTestExpression(test.Segment, tType);
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return test;
            yield return consequent;
            yield return alternate;
        }

        protected override string NodeRepresentation() => "?:";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            var instructions = new List<Instruction>();
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

            var cOffset = start + instructions.Count + 1;
            var consequentInstructions = consequent.ToInstructions(cOffset, temp);

            var aOffset = consequentInstructions.Last().Number + 2;
            var alternateInstructions = alternate.ToInstructions(aOffset, temp);

            instructions.Add(
                new IfNotGotoInstruction(
                    ifNotTest, alternateInstructions.First().Number, cOffset - 1
                )
            );
            instructions.AddRange(consequentInstructions);
            instructions.OfType<ThreeAddressCodeInstruction>().Last().Left = temp;

            instructions.Add(
                new GotoInstruction(alternateInstructions.Last().Number + 1, aOffset - 1)
            );
            instructions.AddRange(alternateInstructions);

            return instructions;
        }
    }
}
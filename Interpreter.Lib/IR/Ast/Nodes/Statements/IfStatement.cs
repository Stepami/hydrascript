using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements
{
    public class IfStatement : Statement
    {
        private readonly Expression _test;
        private readonly Statement _then;
        private readonly Statement _else;

        public IfStatement(Expression test, Statement then, Statement @else = null)
        {
            _test = test;
            _test.Parent = this;

            _then = then;
            _then.Parent = this;

            if (@else != null)
            {
                _else = @else;
                _else.Parent = this;
            }

            CanEvaluate = true;
        }

        public bool HasReturnStatement()
        {
            var thenResult = _then is ReturnStatement;
            if (!thenResult)
            {
                if (_then is BlockStatement block)
                {
                    thenResult = block.HasReturnStatement();
                }
            }

            var elseResult = _else == null || _else is ReturnStatement;
            if (!elseResult)
            {
                if (_else is BlockStatement block)
                {
                    elseResult = block.HasReturnStatement();
                }
            }

            return thenResult && elseResult;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _test;
            yield return _then;
            if (_else != null)
            {
                yield return _else;
            }
        }

        internal override Type NodeCheck()
        {
            var testType = _test.NodeCheck();
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
            if (_then.Any() && (_else == null || _else.Any()))
            {
                IValue ifNotTest;
                if (!_test.Primary())
                {
                    var testInstructions = _test.ToInstructions(start, "_t");
                    ifNotTest = new Name(testInstructions.OfType<Simple>().Last().Left);
                    instructions.AddRange(testInstructions);
                }
                else
                {
                    ifNotTest = ((PrimaryExpression) _test).ToValue();
                }

                var tOffset = start + instructions.Count + 1;
                var thenInstructions = _then.ToInstructions(tOffset);

                var eOffset = thenInstructions.Any()
                    ? thenInstructions.Last().Number + 2
                    : tOffset + 1;
                var elseInstructions = _else?.ToInstructions(eOffset);

                instructions.Add(
                    new IfNotGoto(
                        ifNotTest, elseInstructions?.First().Number ?? eOffset - 1, tOffset - 1
                    )
                );

                instructions.AddRange(thenInstructions);

                if (elseInstructions != null)
                {
                    instructions.Add(
                        new Goto(
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
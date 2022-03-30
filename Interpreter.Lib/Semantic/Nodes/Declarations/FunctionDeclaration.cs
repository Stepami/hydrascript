using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions;
using Interpreter.Lib.Semantic.Nodes.Statements;
using Interpreter.Lib.Semantic.Symbols;

namespace Interpreter.Lib.Semantic.Nodes.Declarations
{
    public class FunctionDeclaration : Declaration
    {
        private readonly FunctionSymbol _function;

        private readonly BlockStatement _statements;

        public FunctionDeclaration(FunctionSymbol function, BlockStatement statements)
        {
            _function = function;
            function.Body = this;

            _statements = statements;
            _statements.Parent = this;
        }

        public bool HasReturnStatement() => _statements.HasReturnStatement();

        public void SetArguments(CallExpression call, List<Expression> expressions)
        {
            if (_function.Parameters.Count == expressions.Count)
            {
                _function.Parameters.Zip(expressions).ToList()
                    .ForEach(tuple =>
                    {
                        var (param, expr) = tuple;
                        if (param is VariableSymbol p)
                        {
                            var pType = expr.NodeCheck();
                            if (p.Type.Equals(pType))
                            {
                                SymbolTable.AddSymbol(p);
                            }
                            else throw new WrongTypeOfArgument(expr.Segment, p.Type, pType);
                        }
                    });
            }
            else throw new WrongNumberOfArguments(call.Segment, _function.Parameters.Count, expressions.Count);
        }

        public void Clear()
        {
            _statements.SymbolTable.Clear();
            SymbolTable.Clear();
        }

        public FunctionSymbol GetSymbol() => _function;

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _statements;
        }

        protected override string NodeRepresentation() => $"function {_function.Id}";

        public override List<Instruction> ToInstructions(int start)
        {
            var instructions = new List<Instruction>();
            if (_statements.Any())
            {
                _function.CallInfo.Location = start + 1;

                var body = new List<Instruction>();
                body.AddRange(_statements.ToInstructions(_function.CallInfo.Location));
                if (!_statements.HasReturnStatement())
                {
                    body.Add(new ReturnInstruction(_function.CallInfo.Location, body.Last().Number + 1));
                }

                instructions.Add(new GotoInstruction(body.Last().Number + 1, start));

                instructions.AddRange(body);
            }

            return instructions;
        }
    }
}
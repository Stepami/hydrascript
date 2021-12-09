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
        private readonly FunctionSymbol function;

        private readonly BlockStatement statements;

        public FunctionDeclaration(FunctionSymbol function, BlockStatement statements)
        {
            this.function = function;
            function.Body = this;

            this.statements = statements;
            this.statements.Parent = this;
        }

        public bool HasReturnStatement() => statements.HasReturnStatement();

        public void SetArguments(List<Expression> expressions)
        {
            if (function.Parameters.Count == expressions.Count)
            {
                function.Parameters.Zip(expressions).ToList()
                    .ForEach(tuple =>
                    {
                        var (param, expr) = tuple;
                        if (param is VariableSymbol p)
                        {
                            var pType = expr.NodeCheck();
                            if (p.Type.Equals(pType) || pType.SubTypeOf(p.Type))
                            {
                                SymbolTable.AddSymbol(p);
                            }
                            else throw new WrongTypeOfArgument(expr.Segment, p.Type, pType);
                        }
                    });
            }
            else throw new WrongNumberOfArguments(Segment, function.Parameters.Count, expressions.Count);
        }

        public void Clear()
        {
            statements.SymbolTable.Clear();
            SymbolTable.Clear();
        }

        public FunctionSymbol GetSymbol() => function;

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return statements;
        }

        protected override string NodeRepresentation() => $"function {function.Id}";

        public override List<Instruction> ToInstructions(int start)
        {
            var instructions = new List<Instruction>();
            if (statements.Any())
            {
                function.CallInfo.Location = start + 1;

                var body = new List<Instruction>();
                body.AddRange(statements.ToInstructions(function.CallInfo.Location));
                if (!statements.HasReturnStatement())
                {
                    body.Add(new ReturnInstruction(function.CallInfo.Location, body.Last().Number + 1));
                }

                instructions.Add(new GotoInstruction(body.Last().Number + 1, start));

                instructions.AddRange(body);
            }

            return instructions;
        }
    }
}
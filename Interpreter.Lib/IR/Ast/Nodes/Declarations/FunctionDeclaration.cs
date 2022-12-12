using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Nodes.Statements;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Declarations
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
            if (_function.Type.Arguments.Count == expressions.Count)
            {
                expressions.Select((e, i) => (e, i)).ToList()
                    .ForEach(pair =>
                    {
                        var (e, i) = pair;
                        var eType = e.NodeCheck();
                        if (_function.Type.Arguments[i].Equals(eType))
                        {
                            SymbolTable.AddSymbol(_function.Parameters[i]);
                        }
                        else throw new WrongTypeOfArgument(e.Segment, _function.Type.Arguments[i], eType);
                    });
            }
            else throw new WrongNumberOfArguments(call.Segment, _function.Parameters.Count, expressions.Count);
        }

        public void Clear()
        {
            _statements.GetAllNodes().ForEach(x => x.SymbolTable?.Clear());
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

                var body = new List<Instruction>
                {
                    new BeginFunction(_function.CallInfo.Location, _function.CallInfo)
                };
                body.AddRange(_statements.ToInstructions(_function.CallInfo.Location + 1));
                if (!_statements.HasReturnStatement())
                {
                    body.Add(new Return(_function.CallInfo.Location, body.Last().Number + 1));
                }

                instructions.Add(new Goto(body.Last().Number + 1, start));

                instructions.AddRange(body);
            }

            return instructions;
        }
    }
}
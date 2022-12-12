using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions
{
    public class IdentifierReference : PrimaryExpression
    {
        public string Id { get; }

        public IdentifierReference(string id)
        {
            Id = id;
        }

        internal override Type NodeCheck()
        {
            if (!ChildOf<DotAccess>())
            {
                var symbol = SymbolTable.FindSymbol<Symbol>(Id);
                return symbol switch
                {
                    VariableSymbol v => v.Type,
                    FunctionSymbol f => f.Type,
                    _ => throw new UnknownIdentifierReference(this)
                };
            }

            return null;
        }

        protected override string NodeRepresentation() => Id;

        public override IValue ToValue() => new Name(Id);
    }
}
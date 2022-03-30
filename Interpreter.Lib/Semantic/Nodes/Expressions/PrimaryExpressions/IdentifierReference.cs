using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.VM;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions
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
            var symbol = SymbolTable.FindSymbol<Symbol>(Id);
            return symbol switch
            {
                VariableSymbol v => v.Type,
                FunctionSymbol f => f.Type,
                _ => throw new UnknownIdentifierReference(this)
            };
        }

        protected override string NodeRepresentation() => Id;

        public override IValue ToValue() => new Name(Id);
    }
}
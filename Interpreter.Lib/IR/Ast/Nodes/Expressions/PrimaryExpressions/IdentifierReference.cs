using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;

public class IdentifierReference : PrimaryExpression
{
    public string Name { get; }

    public IdentifierReference(string name)
    {
        Name = name;
    }

    internal override Type NodeCheck()
    {
        if (!ChildOf<DotAccess>())
        {
            var symbol = SymbolTable.FindSymbol<Symbol>(Name);
            return symbol switch
            {
                VariableSymbol v => v.Type,
                FunctionSymbol f => f.Type,
                _ => throw new UnknownIdentifierReference(this)
            };
        }

        return null;
    }

    protected override string NodeRepresentation() => Name;

    public override IValue ToValue() => new Name(Name);

    public static implicit operator string(IdentifierReference identifierReference) =>
        identifierReference.Name;
}
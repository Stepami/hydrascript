using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public class MemberExpression : LeftHandSideExpression
{
    private readonly IdentifierReference _identifierReference;

    public AccessExpression AccessChain { get; }
    public AccessExpression Tail { get; }

    public MemberExpression(IdentifierReference identifierReference,
        AccessExpression accessChain = null, AccessExpression tail = null)
    {
        _identifierReference = identifierReference;
        _identifierReference.Parent = this;
            
        AccessChain = accessChain;
        if (accessChain is not null)
        {
            AccessChain.Parent = this;
        }

        Tail = tail;
    }

    public override IdentifierReference Id =>
        _identifierReference;

    public override bool Empty() =>
        AccessChain is null;

    internal override Type NodeCheck()
    {
        if (AccessChain == null)
        {
            return _identifierReference.NodeCheck();
        }

        var symbol = SymbolTable.FindSymbol<VariableSymbol>(_identifierReference);
        if (symbol == null)
        {
            throw new UnknownIdentifierReference(_identifierReference);
        }

        return AccessChain.Check(symbol.Type);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        if (AccessChain is not null)
        {
            yield return AccessChain;
        }
    }

    protected override string NodeRepresentation() => Id;

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}
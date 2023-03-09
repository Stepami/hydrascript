using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public class MemberExpression : LeftHandSideExpression
{
    private readonly IdentifierReference _identifierReference;
        
    public AccessExpression AccessChain { get; }

    public MemberExpression(IdentifierReference identifierReference, AccessExpression accessChain)
    {
        _identifierReference = identifierReference;
        _identifierReference.Parent = this;
            
        AccessChain = accessChain;
        if (accessChain != null)
        {
            AccessChain.Parent = this;
        }
    }

    public override IdentifierReference Id =>
        _identifierReference;

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
        if (AccessChain != null)
        {
            yield return AccessChain;
        }
    }

    protected override string NodeRepresentation() => Id;

    /*public List<Instruction> ToInstructions(int start, string temp)
    {
        if (AccessChain != null && AccessChain.HasNext())
        {
            return AccessChain.ToInstructions(start, _id.Id);
        }

        return new();
    }*/

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor)
    {
        throw new NotImplementedException();
    }
}
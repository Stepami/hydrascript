using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public class MemberExpression : Expression
{
    private readonly IdentifierReference _id;
        
    public AccessExpression AccessChain { get; }

    public MemberExpression(IdentifierReference id, AccessExpression accessChain)
    {
        _id = id;
        _id.Parent = this;
            
        AccessChain = accessChain;
        if (accessChain != null)
        {
            AccessChain.Parent = this;
        }
    }

    public string Id => _id.Id;

    internal override Type NodeCheck()
    {
        if (AccessChain == null)
        {
            return _id.NodeCheck();
        }

        var symbol = SymbolTable.FindSymbol<VariableSymbol>(_id.Id);
        if (symbol == null)
        {
            throw new UnknownIdentifierReference(_id);
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

    public static implicit operator IdentifierReference(MemberExpression member) => 
        member._id;
        
    public static explicit operator MemberExpression(IdentifierReference idRef) =>
        new (idRef, null);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor)
    {
        throw new NotImplementedException();
    }
}
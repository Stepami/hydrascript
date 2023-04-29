using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;

public class TypeDeclaration : Declaration
{
    private readonly string _typeId;
    private readonly Type _typeValue;

    public TypeDeclaration(string typeId, Type typeValue)
    {
        _typeId = typeId;
        _typeValue = typeValue;
    }
        
    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield break;
    }

    protected override string NodeRepresentation() =>
        $"type {_typeId} = {_typeValue}";

    public override AddressedInstructions Accept(InstructionProvider visitor) => new();
    
    public override Unit Accept(DeclarationVisitor visitor) =>
        visitor.Visit(this);
}
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Visitor.NET;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;

public class TypeDeclaration : Declaration
{
    public string TypeId { get; }
    public Type TypeValue { get; }

    public TypeDeclaration(string typeId, Type typeValue)
    {
        TypeId = typeId;
        TypeValue = typeValue;
    }
        
    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield break;
    }

    protected override string NodeRepresentation() =>
        $"type {TypeId} = {TypeValue}";

    public override AddressedInstructions Accept(InstructionProvider visitor) => new();
    
    public override Unit Accept(DeclarationVisitor visitor) =>
        visitor.Visit(this);
}
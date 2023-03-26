using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;

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
}
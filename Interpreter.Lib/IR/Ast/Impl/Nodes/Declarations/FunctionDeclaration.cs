using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer;
using Visitor.NET;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;

public class FunctionDeclaration : Declaration
{
    public string Name { get; }
    public TypeValue ReturnTypeValue { get; }
    public List<PropertyTypeValue> Arguments { get; }
    public BlockStatement Statements { get; }

    public ObjectLiteral Object =>
        Parent as ObjectLiteral;

    public FunctionDeclaration(
        string name,
        TypeValue returnTypeValue,
        List<PropertyTypeValue> arguments,
        BlockStatement blockStatement)
    {
        Name = name;
        ReturnTypeValue = returnTypeValue;
        Arguments = arguments;

        Statements = blockStatement;
        Statements.Parent = this;
    }

    public bool HasReturnStatement() =>
        Statements.GetAllNodes()
            .OfType<ReturnStatement>()
            .Any();

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Statements;
    }

    protected override string NodeRepresentation() =>
        $"function {Name}";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);

    public override Unit Accept(SymbolTableInitializer visitor) =>
        visitor.Visit(this);
    
    public override Unit Accept(DeclarationVisitor visitor) =>
        visitor.Visit(this);
}
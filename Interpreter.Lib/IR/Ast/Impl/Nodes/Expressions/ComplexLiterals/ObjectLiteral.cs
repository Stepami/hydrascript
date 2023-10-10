using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer;
using Visitor.NET;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

public class ObjectLiteral : ComplexLiteral
{
    public List<Property> Properties { get; }
    public List<FunctionDeclaration> Methods { get; }

    public ObjectLiteral(IEnumerable<Property> properties, IEnumerable<FunctionDeclaration> methods)
    {
        Properties = new List<Property>(properties);
        Properties.ForEach(prop => prop.Parent = this);

        Methods = new List<FunctionDeclaration>(methods);
        Methods.ForEach(m => m.Parent = this);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Properties.Concat<AbstractSyntaxTreeNode>(Methods).GetEnumerator();

    protected override string NodeRepresentation() => "{}";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        new InstructionProvider().Visit(this);

    public override Unit Accept(SymbolTableInitializer visitor) =>
        visitor.Visit(this);
}
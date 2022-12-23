using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Nodes.Declarations;

public class LexicalDeclaration : Declaration
{
    public bool Readonly { get; }
    public List<AssignmentExpression> Assignments { get; }

    public LexicalDeclaration(bool @readonly)
    {
        Readonly = @readonly;
        Assignments = new();
    }

    public void AddAssignment(AssignmentExpression assignment)
    {
        assignment.SymbolTable = SymbolTable;
        assignment.Parent = this;
        Assignments.Add(assignment);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Assignments.GetEnumerator();

    protected override string NodeRepresentation() =>
        Readonly ? "const" : "let";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}
using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;

public class LexicalDeclaration : AfterTypesAreLoadedDeclaration
{
    public bool ReadOnly { get; }
    public List<AssignmentExpression> Assignments { get; }

    public LexicalDeclaration(bool readOnly)
    {
        ReadOnly = readOnly;
        Assignments = new();
    }

    public void AddAssignment(AssignmentExpression assignment)
    {
        assignment.Parent = this;
        Assignments.Add(assignment);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Assignments.GetEnumerator();

    protected override string NodeRepresentation() =>
        ReadOnly ? "const" : "let";

    public override Unit Accept(DeclarationVisitor visitor) =>
        visitor.Visit(this);

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class LexicalDeclaration(bool readOnly) : AfterTypesAreLoadedDeclaration
{
    public bool ReadOnly { get; } = readOnly;
    public List<AssignmentExpression> Assignments { get; } = [];

    public void AddAssignment(AssignmentExpression assignment)
    {
        assignment.Parent = this;
        Assignments.Add(assignment);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Assignments.GetEnumerator();

    protected override string NodeRepresentation() =>
        ReadOnly ? "const" : "let";
}
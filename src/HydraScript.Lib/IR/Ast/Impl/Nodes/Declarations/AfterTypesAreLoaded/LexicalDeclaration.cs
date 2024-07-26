using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class LexicalDeclaration(bool readOnly) : AfterTypesAreLoadedDeclaration
{
    private readonly List<AssignmentExpression> _assignments = [];
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        _assignments;

    public bool ReadOnly { get; } = readOnly;
    public IReadOnlyList<AssignmentExpression> Assignments =>
        _assignments;

    public void AddAssignment(AssignmentExpression assignment)
    {
        assignment.Parent = this;
        _assignments.Add(assignment);
    }

    protected override string NodeRepresentation() =>
        ReadOnly ? "const" : "let";
}
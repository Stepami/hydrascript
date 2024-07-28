using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class LexicalDeclaration(bool readOnly) : AfterTypesAreLoadedDeclaration
{
    private readonly List<AssignmentExpression> _assignments = [];
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
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
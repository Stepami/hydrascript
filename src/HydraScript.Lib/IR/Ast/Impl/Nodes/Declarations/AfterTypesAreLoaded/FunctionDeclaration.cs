using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class FunctionDeclaration : AfterTypesAreLoadedDeclaration
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children => [Statements];

    public IdentifierReference Name { get; }
    public TypeValue ReturnTypeValue { get; }
    public List<PropertyTypeValue> Arguments { get; }
    public BlockStatement Statements { get; }

    public FunctionDeclaration(
        IdentifierReference name,
        TypeValue returnTypeValue,
        List<PropertyTypeValue> arguments,
        BlockStatement blockStatement)
    {
        Name = name;
        ReturnTypeValue = returnTypeValue;
        Arguments = arguments;

        Statements = blockStatement;
        Statements.Parent = this;

        ReturnStatements = Statements
            .GetAllNodes()
            .OfType<ReturnStatement>()
            .ToArray();
    }

    public bool HasReturnStatement() =>
        ReturnStatements.Count > 0;

    public IReadOnlyCollection<ReturnStatement> ReturnStatements { get; }

    protected override string NodeRepresentation() =>
        "function " + Name;
}
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class FunctionDeclaration : AfterTypesAreLoadedDeclaration
{
    private IReadOnlyCollection<ReturnStatement>? _returnStatements;

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
    }

    public bool HasReturnStatement()
    {
        _returnStatements ??= GetReturnStatements();
        return _returnStatements.Count > 0;
    }

    public IReadOnlyCollection<ReturnStatement> GetReturnStatements() =>
        _returnStatements ??= Statements
            .GetAllNodes()
            .OfType<ReturnStatement>()
            .ToArray();

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Statements;
    }

    protected override string NodeRepresentation() =>
        "function " + Name;
}
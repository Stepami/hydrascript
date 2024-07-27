using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class FunctionDeclaration : AfterTypesAreLoadedDeclaration
{
    private readonly List<PropertyTypeValue> _arguments;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children => [Statements];

    public IdentifierReference Name { get; }
    public TypeValue ReturnTypeValue { get; }
    public IReadOnlyList<PropertyTypeValue> Arguments => _arguments;

    public BlockStatement Statements { get; }
    public bool IsEmpty => Statements.Count == 0;

    public FunctionDeclaration(
        IdentifierReference name,
        TypeValue returnTypeValue,
        List<PropertyTypeValue> arguments,
        BlockStatement blockStatement)
    {
        Name = name;
        ReturnTypeValue = returnTypeValue;
        _arguments = arguments;

        Statements = blockStatement;
        Statements.Parent = this;

        ReturnStatements = Statements
            .GetAllNodes()
            .OfType<ReturnStatement>()
            .ToArray();
    }

    /// <summary>Стратегия "блока" - углубление скоупа</summary>
    /// <param name="scope">Новый скоуп</param>
    public override void InitScope(Scope? scope = null)
    {
        ArgumentNullException.ThrowIfNull(scope);
        Scope = scope;
        Scope.AddOpenScope(Parent.Scope);

        _arguments.ForEach(x => x.TypeValue.Scope = Parent.Scope);
        ReturnTypeValue.Scope = Parent.Scope;
    }

    public bool HasReturnStatement() =>
        ReturnStatements.Count > 0;

    public IReadOnlyCollection<ReturnStatement> ReturnStatements { get; }

    protected override string NodeRepresentation() =>
        "function " + Name;
}
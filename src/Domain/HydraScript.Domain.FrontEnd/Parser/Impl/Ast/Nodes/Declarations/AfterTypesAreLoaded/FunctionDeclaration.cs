using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class FunctionDeclaration : AfterTypesAreLoadedDeclaration
{
    private readonly List<IFunctionArgument> _arguments;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children => [Statements];

    public IdentifierReference Name { get; }
    public TypeValue ReturnTypeValue { get; }
    public IReadOnlyList<IFunctionArgument> Arguments => _arguments;

    public BlockStatement Statements { get; }
    public bool IsEmpty => Statements.Count == 0;

    public string ComputedFunctionAddress { get; set; } = default!;

    public FunctionDeclaration(
        IdentifierReference name,
        TypeValue returnTypeValue,
        List<IFunctionArgument> arguments,
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
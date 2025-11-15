namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;

public abstract class ComplexLiteral : Expression
{
    private string? _nullId;

    protected string NullId
    {
        get
        {
            _nullId ??= $"{GetHashCode()}";
            return _nullId;
        }
    }

    public abstract string Id { get; }
}
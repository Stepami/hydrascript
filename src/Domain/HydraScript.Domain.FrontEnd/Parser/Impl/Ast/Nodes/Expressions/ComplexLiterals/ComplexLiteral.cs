namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;

public abstract class ComplexLiteral : Expression
{
    protected string NullId
    {
        get
        {
            field ??= $"{GetHashCode()}";
            return field;
        }
    }

    public abstract string Id { get; }
}
using Cysharp.Text;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;

public abstract class ComplexLiteral : Expression
{
    protected abstract string NullIdPrefix { get; }

    protected string NullId
    {
        get
        {
            field ??= ZString.Concat("_t_", NullIdPrefix, '_', GetHashCode());
            return field;
        }
    }

    public abstract IdentifierReference Id { get; }
}
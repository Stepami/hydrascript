using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.IR.Types.Operators;

namespace HydraScript.Domain.IR.Types;

public class Type(string name, List<IOperator>? operators = null) : IEquatable<Type>
{
    private readonly string _name = name;
    private readonly Dictionary<string, IOperator> _operators = GetOperators(operators ?? []);

    private static Dictionary<string, IOperator> GetOperators(List<IOperator> operators)
    {
        operators.Add(default(EqualityOperator));
        Dictionary<string, IOperator> operatorsDictionary = [];

        for (var i = 0; i < operators.Count; i++)
        {
            var @operator = operators[i];
            for (var j = 0; j < @operator.Values.Count; j++)
                operatorsDictionary[@operator.Values[j]] = @operator;
        }

        return operatorsDictionary;
    }

    public virtual void ResolveReference(
        Type reference,
        string refId,
        ISet<Type>? visited = null)
    {
    }

    public virtual bool TryGetOperator(string value, [MaybeNullWhen(false)] out IOperator @operator) =>
        _operators.TryGetValue(value, out @operator);

    public virtual bool Equals(Type? obj) =>
        obj switch
        {
            Any => true,
            not null => _name == obj._name,
            _ => false
        };

    public override bool Equals(object? obj) => Equals(obj as Type);

    public override int GetHashCode() =>
        _name.GetHashCode();

    public override string ToString() => _name;

    public static implicit operator Type(string alias) =>
        new(alias);

    public static bool operator ==(Type left, Type right) =>
        Equals(left, right);

    public static bool operator !=(Type left, Type right) =>
        !(left == right);
}
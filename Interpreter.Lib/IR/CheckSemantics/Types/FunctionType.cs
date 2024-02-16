using System.Text;

namespace Interpreter.Lib.IR.CheckSemantics.Types;

public class FunctionType : Type
{
    public Type ReturnType { get; private set; }

    private readonly List<Type> _arguments;
    public IReadOnlyList<Type> Arguments => _arguments;

    public FunctionType(Type returnType, IEnumerable<Type> arguments)
    {
        ReturnType = returnType;
        _arguments = new List<Type>(arguments);
    }

    public void DefineReturnType(Type returnType) =>
        ReturnType = returnType;

    public override void ResolveReference(
        Type reference,
        string refId,
        ISet<Type> visited = null)
    {
        if (ReturnType == refId)
            ReturnType = reference;
        else
            ReturnType.ResolveReference(reference, refId, visited);

        for (var i = 0; i < Arguments.Count; i++)
        {
            var argType = Arguments[i];
            if (argType == refId)
                _arguments[i] = reference;
            else
                argType.ResolveReference(reference, refId, visited);
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is FunctionType that)
            return ReturnType.Equals(that.ReturnType) &&
                   Arguments.Count == that.Arguments.Count &&
                   Arguments.Zip(that.Arguments)
                       .All(pair => pair.First.Equals(pair.Second));

        return obj is Any;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            ReturnType,
            Arguments
                .Select(arg => arg.GetHashCode())
                .Aggregate(36, HashCode.Combine)
        );

    public override string ToString() =>
        new StringBuilder()
            .Append('(')
            .AppendJoin(", ", Arguments)
            .Append(')')
            .Append(" => ")
            .Append(ReturnType)
            .ToString();
}
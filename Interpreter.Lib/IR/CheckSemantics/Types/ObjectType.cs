using System.Text;

namespace Interpreter.Lib.IR.CheckSemantics.Types;

public class ObjectType : NullableType
{
    private readonly Dictionary<string, Type> _properties;
    private readonly ObjectTypeHasher _hasher;
    private readonly ObjectTypePrinter _serializer;

    public ObjectType(IEnumerable<PropertyType> properties)
    {
        _properties = properties
            .OrderBy(x => x.Id)
            .ToDictionary(
                x => x.Id,
                x => x.Type);

        _hasher = new ObjectTypeHasher(this);
        _serializer = new ObjectTypePrinter(this);
    }

    public Type this[string id]
    {
        get => _properties.GetValueOrDefault(id);
        private set => _properties[id] = value;
    }

    public override void ResolveReference(
        Type reference,
        string refId,
        ISet<Type> visited = null)
    {
        visited ??= new HashSet<Type>();
        if (!visited.Add(this))
            return;

        foreach (var key in _properties.Keys)
            if (refId == this[key])
                this[key] = reference;
            else
                this[key].ResolveReference(reference, refId, visited);
    }

    public override bool Equals(object obj)
    {
        if (obj is ObjectType that)
            return ReferenceEquals(this, that) || _properties.Count == that._properties.Count &&
                _properties
                    .Zip(that._properties).All(
                        pair =>
                            pair.First.Key == pair.Second.Key &&
                            pair.First.Value.Equals(pair.Second.Value));

        return obj is NullType or Any;
    }

    public override int GetHashCode() =>
        _hasher.HashObjectType(this);

    public override string ToString()
    {
        var result = _serializer.PrintObjectType(this);
        _serializer.Clear();
        return result;
    }

    private class ObjectTypeHasher
    {
        private readonly ObjectType _reference;

        public ObjectTypeHasher(ObjectType reference) =>
            _reference = reference;

        private int Hash(Type type) => type switch
        {
            ArrayType arrayType => HashArrayType(arrayType),
            FunctionType functionType => HashFunctionType(functionType),
            ObjectType objectType => HashObjectType(objectType),
            NullableType nullableType => HashNullableType(nullableType),
            _ => type.GetHashCode()
        };

        public int HashObjectType(ObjectType objectType) =>
            objectType._properties.Keys.Select(
                    key => HashCode.Combine(
                        key,
                        objectType[key].Equals(_reference)
                            ? "@this".GetHashCode()
                            : objectType[key].GetType().GetHashCode()))
                .Aggregate(36, HashCode.Combine);

        private int HashArrayType(ArrayType arrayType) =>
            arrayType.Type.Equals(_reference)
                ? "@this".GetHashCode()
                : Hash(arrayType.Type);

        private int HashNullableType(NullableType nullableType) =>
            nullableType.Type.Equals(_reference)
                ? "@this".GetHashCode()
                : Hash(nullableType.Type);

        private int HashFunctionType(FunctionType functionType) =>
            HashCode.Combine(
                functionType.ReturnType.Equals(_reference)
                    ? "@this".GetHashCode()
                    : Hash(functionType.ReturnType),
                functionType.Arguments.Select(
                        arg => arg.Equals(_reference)
                            ? "@this".GetHashCode()
                            : Hash(arg))
                    .Aggregate(36, HashCode.Combine));
    }

    private class ObjectTypePrinter
    {
        private readonly ObjectType _reference;
        private readonly ISet<Type> _visited;

        public ObjectTypePrinter(ObjectType reference)
        {
            _reference = reference;
            _visited = new HashSet<Type>();
        }

        public void Clear() => _visited.Clear();

        private string Print(Type type) => type switch
        {
            ArrayType arrayType => PrintArrayType(arrayType),
            FunctionType functionType => PrintFunctionType(functionType),
            ObjectType objectType => PrintObjectType(objectType),
            NullableType nullableType => PrintNullableType(nullableType),
            _ => type.ToString()
        };

        public string PrintObjectType(ObjectType objectType)
        {
            if (_visited.Contains(objectType))
                return string.Empty;
            if (!objectType.Equals(_reference))
                _visited.Add(objectType);

            var sb = new StringBuilder("{");
            foreach (var key in objectType._properties.Keys)
            {
                var type = objectType[key];
                var prop = $"{key}: ";

                if (type.Equals(_reference))
                    prop += "@this";
                else
                {
                    var printedType = Print(type);
                    prop += string.IsNullOrEmpty(printedType)
                        ? key
                        : printedType;
                }

                sb.Append(prop).Append(';');
            }

            return sb.Append('}').ToString();
        }

        private string PrintArrayType(ArrayType arrayType)
        {
            var sb = new StringBuilder();
            sb.Append(arrayType.Type.Equals(_reference)
                ? "@this"
                : Print(arrayType.Type));

            return sb.Append("[]").ToString();
        }

        private string PrintNullableType(NullableType nullableType)
        {
            var sb = new StringBuilder();
            sb.Append(nullableType.Type.Equals(_reference)
                ? "@this"
                : Print(nullableType.Type));

            return sb.Append('?').ToString();
        }

        private string PrintFunctionType(FunctionType functionType)
        {
            var sb = new StringBuilder("(");
            sb.AppendJoin(
                    ", ",
                    functionType.Arguments.Select(
                        x => x.Equals(_reference)
                            ? "@this"
                            : Print(x)))
                .Append(") => ");
            sb.Append(
                functionType.ReturnType.Equals(_reference)
                    ? "@this"
                    : Print(functionType.ReturnType));

            return sb.ToString();
        }
    }
}

public record PropertyType(string Id, Type Type);
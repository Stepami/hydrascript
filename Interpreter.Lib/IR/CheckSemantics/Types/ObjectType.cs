using Interpreter.Lib.IR.CheckSemantics.Types.Visitors;
using Visitor.NET;

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
                x => x.Type
            );
        _hasher = new ObjectTypeHasher(this);
        _serializer = new ObjectTypePrinter(this);
    }

    public Type this[string id]
    {
        get => _properties.ContainsKey(id)
            ? _properties[id]
            : null;
        set => _properties[id] = value;
    }

    public IEnumerable<string> Keys => _properties.Keys;

    public void ResolveSelfReferences(string self) =>
        new ReferenceResolver(this, self)
            .Visit(this);

    public override Unit Accept(ReferenceResolver visitor) =>
        visitor.Visit(this);
        
    public override string Accept(ObjectTypePrinter visitor) =>
        visitor.Visit(this);
        
    public override int Accept(ObjectTypeHasher visitor) =>
        visitor.Visit(this);

    public override bool Equals(object obj)
    {
        if (obj is ObjectType that)
        {
            return ReferenceEquals(this, that) || _properties.Count == that._properties.Count &&
                _properties
                    .Zip(that._properties)
                    .All(pair =>
                        pair.First.Key == pair.Second.Key &&
                        pair.First.Value.Equals(pair.Second.Value)
                    );
        }

        return obj is NullType;
    }

    public override int GetHashCode() =>
        _hasher.Visit(this);

    public override string ToString() =>
        _serializer.Visit(this);
}

public record PropertyType(string Id, Type Type);
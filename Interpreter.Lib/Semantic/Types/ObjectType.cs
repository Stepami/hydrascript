using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Lib.Semantic.Types
{
    public class ObjectType : NullableType
    {
        private readonly Dictionary<string, Type> _properties;

        public ObjectType(IEnumerable<PropertyType> properties)
        {
            _properties = properties
                .OrderBy(x => x.Id)
                .ToDictionary(
                    x => x.Id,
                    x => x.Type
                );
        }

        public Type this[string id] => _properties.ContainsKey(id)
            ? _properties[id]
            : null;

        public void ResolveSelfReferences(string self)
        {
            foreach (var (key, property) in _properties)
            {
                if (property == self)
                {
                    _properties[key] = this;
                } 
                else switch (property)
                {
                    case ObjectType objectType:
                        objectType.ResolveSelfReferences(self);
                        break;
                    default:
                        property.ResolveReference(self, this);
                        break;
                }
            }
        }
        
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
            _properties
                .Select(kvp => HashCode.Combine(kvp.Key, kvp.Value))
                .Aggregate(HashCode.Combine);

        public override string ToString() =>
            new StringBuilder()
                .Append('{')
                .AppendJoin(
                    "",
                    _properties
                        .Select(kvp =>
                            $"{kvp.Key}: {kvp.GetHashCode()};")
                )
                .Append('}')
                .ToString();
    }

    public record PropertyType(string Id, Type Type);
}
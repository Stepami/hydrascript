using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Lib.Semantic.Types
{
    public class ObjectType : Type
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

        public Type this[string id] => _properties[id];

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var that = (ObjectType) obj;
            return _properties.Count == that._properties.Count &&
                   _properties
                       .Zip(that._properties)
                       .All(pair =>
                           pair.First.Key == pair.Second.Key &&
                           pair.First.Value.Equals(pair.Second.Value)
                       );
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
                        .Select(kvp => $"{kvp.Key}: {kvp.Value};")
                )
                .Append('}')
                .ToString();
    }

    public record PropertyType(string Id, Type Type);
}
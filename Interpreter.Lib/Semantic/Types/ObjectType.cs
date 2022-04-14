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

        public override bool Equals(object obj)
        {
            if (obj is ObjectType that)
            {
                return this == that || _properties.Count == that._properties.Count &&
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
                            $"{kvp.Key}: {(kvp.Value.Equals(this) ? "this" : kvp.Value.ToString())};")
                )
                .Append('}')
                .ToString();

        public static ObjectType RecursiveFromProperties(params PropertyType[] propertyTypes)
        {
            var propList = propertyTypes.ToList();
            var objectType = new ObjectType(propList.Where(x => !x.Recursive));
            foreach (var prop in propList.Where(x=>x.Recursive))
            {
                objectType._properties[prop.Id] = objectType;
            }

            return objectType;
        }
    }

    public record PropertyType(string Id, Type Type, bool Recursive = false);

    public record RecursivePropertyType(string Id, Type Type = null, bool Recursive = true) :
        PropertyType(Id, Type, Recursive);
}
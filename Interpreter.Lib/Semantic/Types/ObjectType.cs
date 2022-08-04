using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interpreter.Lib.Semantic.Types.Visitors;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.Semantic.Types
{
    public class ObjectType : NullableType
    {
        private readonly Dictionary<string, Type> _properties;
        private readonly ObjectTypeToStringSerializer _serializer;

        public ObjectType(IEnumerable<PropertyType> properties)
        {
            _properties = properties
                .OrderBy(x => x.Id)
                .ToDictionary(
                    x => x.Id,
                    x => x.Type
                );
            _serializer = new ObjectTypeToStringSerializer(this);
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

        public override string ToString() => _serializer.Serialize();
        
        private class ObjectTypeToStringSerializer
        {
            private readonly ObjectType _root;
            
            public ObjectTypeToStringSerializer(ObjectType root)
            {
                _root = root;
            }

            private string HandleType(Type type) =>
                type switch
                {
                    ObjectType objectType => HandleObjectType(objectType),
                    _ => type.ToString()
                };

            private string HandleObjectType(ObjectType objectType) =>
                ReferenceEquals(objectType, _root)
                    ? "@this"
                    : SerializeRecursive(objectType);

            private string HandleFunctionType(FunctionType functionType)
            {
                return "";
            }

            private string SerializeRecursive(ObjectType objectType)
            {
                var sb = new StringBuilder("{");
                foreach (var key in objectType.Keys)
                {
                    var value = objectType[key];
                    var prop = $"{key}: ";
                    prop += HandleType(value);

                    sb.Append(prop).Append(';');
                }
                return sb.Append('}').ToString();
            }

            public string Serialize() => SerializeRecursive(_root);
        }
    }

    public record PropertyType(string Id, Type Type);
}
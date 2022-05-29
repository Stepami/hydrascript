using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Lib.Semantic.Types
{
    public class FunctionType : Type
    {
        public Type ReturnType { get; private set; }
        
        public List<Type> Arguments { get; }

        public FunctionType(Type returnType, IEnumerable<Type> arguments)
        {
            ReturnType = returnType;
            Arguments = new List<Type>(arguments);
        }

        public override void ResolveReference(string reference, Type toAssign)
        {
            if (ReturnType.ToString() == reference)
            {
                ReturnType = toAssign;
            } 
            else switch (ReturnType)
            {
                case ObjectType objectType:
                    objectType.ResolveSelfReferences(reference);
                    break;
                default:
                    ReturnType.ResolveReference(reference, toAssign);
                    break;
            }

            for (var i = 0; i < Arguments.Count; i++)
            {
                if (Arguments[i].ToString() == reference)
                {
                    Arguments[i] = toAssign;
                }
                else switch (Arguments[i])
                {
                    case ObjectType objectType:
                        objectType.ResolveSelfReferences(reference);
                        break;
                    default:
                        Arguments[i].ResolveReference(reference, toAssign);
                        break;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var that = (FunctionType) obj;
            return ReturnType.Equals(that.ReturnType) &&
                   Arguments.Count == that.Arguments.Count &&
                   Arguments.Zip(that.Arguments)
                       .All(pair => pair.First.Equals(pair.Second));
        }

        public override int GetHashCode() =>
            HashCode.Combine(
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                ReturnType,
                Arguments
                    .Select(arg => arg.GetHashCode())
                    .Aggregate(HashCode.Combine)
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
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interpreter.Lib.IR.CheckSemantics.Types.Visitors;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Types
{
    public class FunctionType : Type
    {
        public Type ReturnType { get; set; }
        
        public List<Type> Arguments { get; }

        public FunctionType(Type returnType, IEnumerable<Type> arguments)
        {
            ReturnType = returnType;
            Arguments = new List<Type>(arguments);
        }

        public override Unit Accept(ReferenceResolver visitor) =>
            visitor.Visit(this);
        
        public override string Accept(ObjectTypePrinter visitor) =>
            visitor.Visit(this);
        
        public override int Accept(ObjectTypeHasher visitor) =>
            visitor.Visit(this);
        
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
}
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Types.Visitors
{
    public class ObjectTypeHasher : 
        IVisitor<Type, int>,
        IVisitor<ObjectType, int>,
        IVisitor<ArrayType, int>,
        IVisitor<NullableType, int>,
        IVisitor<FunctionType, int>
    {
        private readonly ObjectType _reference;

        public ObjectTypeHasher(ObjectType reference) =>
            _reference = reference;
        
        public int Visit(Type visitable) =>
            visitable.GetHashCode();

        public int Visit(ObjectType visitable) =>
            visitable.Keys.Select(key => HashCode.Combine(key,
                visitable[key].Equals(_reference)
                    ? "@this".GetHashCode()
                    : visitable[key].Recursive
                        ? key.GetHashCode()
                        : visitable[key].Accept(this))
            ).Aggregate(36, HashCode.Combine);

        public int Visit(ArrayType visitable) =>
            visitable.Type.Equals(_reference)
                ? "@this".GetHashCode()
                : visitable.Type.Accept(this);

        public int Visit(NullableType visitable) =>
            visitable.Type.Equals(_reference)
                ? "@this".GetHashCode()
                : visitable.Type.Accept(this);

        public int Visit(FunctionType visitable) =>
            HashCode.Combine(
                visitable.ReturnType.Equals(_reference)
                    ? "@this".GetHashCode()
                    : visitable.ReturnType.Accept(this),
                visitable.Arguments.Select(arg =>
                    arg.Equals(_reference)
                        ? "@this".GetHashCode()
                        : arg.Accept(this)
                ).Aggregate(36, HashCode.Combine));
    }
}
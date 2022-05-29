namespace Interpreter.Lib.Semantic.Types
{
    public class NullableType : Type
    {
        public Type Type { get; private set; }
        
        public NullableType(Type type) : base($"{type}?")
        {
            Type = type;
        }

        protected NullableType()
        {
        }
        
        public override void ResolveReference(string reference, Type toAssign)
        {
            if (Type == reference)
            {
                Type = toAssign;
            }
            else switch (Type)
            {
                case ObjectType objectType:
                    objectType.ResolveSelfReferences(reference);
                    break;
                default:
                    Type.ResolveReference(reference, toAssign);
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is NullableType that)
            {
                return Type.Equals(that.Type);
            }
            return obj is NullType;
        }

        public override int GetHashCode() =>
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            Type.GetHashCode();
    }
}
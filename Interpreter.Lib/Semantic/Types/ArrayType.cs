namespace Interpreter.Lib.Semantic.Types
{
    public class ArrayType : Type
    {
        public Type Type { get; private set; }

        public ArrayType(Type type) : base($"{type}[]")
        {
            Type = type;
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
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var that = (ArrayType) obj;
            return Equals(Type, that.Type);
        }
        
        public override int GetHashCode() => 
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            Type.GetHashCode();
    }
}
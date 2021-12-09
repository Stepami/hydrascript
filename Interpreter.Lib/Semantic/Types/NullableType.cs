namespace Interpreter.Lib.Semantic.Types
{
    public class NullableType : Type
    {
        public Type Type { get; }
        
        public NullableType(Type type) : base($"{type}?")
        {
            CanBeNull = true;
            Type = type;
        }
        
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var that = (NullableType) obj;
            return Equals(Type, that.Type);
        }
        
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}
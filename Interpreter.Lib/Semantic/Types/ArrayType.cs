namespace Interpreter.Lib.Semantic.Types
{
    public class ArrayType : Type
    {
        public Type Type { get; set; }

        public ArrayType(Type type) : base($"{type}[]")
        {
            Type = type;
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
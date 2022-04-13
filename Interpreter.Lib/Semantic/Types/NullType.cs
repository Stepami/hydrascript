namespace Interpreter.Lib.Semantic.Types
{
    public class NullType : Type
    {
        public NullType() : base("null")
        {
        }

        public override bool Equals(object obj)
        {
            return obj is NullableType or NullType;
        }

        public override int GetHashCode() =>
            "null".GetHashCode();
    }
}
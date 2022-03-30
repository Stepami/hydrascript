namespace Interpreter.Lib.Semantic.Types
{
    public class Any : Type
    {
        public Any() : base("any")
        {
        }

        public override bool Equals(object obj) => true;

        public override int GetHashCode() => "any".GetHashCode();
    }
}
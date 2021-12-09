namespace Interpreter.Lib.VM
{
    public class Constant : IValue
    {
        private readonly object value;
        private readonly string representation;

        public Constant(object value, string representation)
        {
            this.value = value;
            this.representation = representation;
        }

        public object Get(Frame frame) => value;

        public override string ToString() => representation;
    }
}
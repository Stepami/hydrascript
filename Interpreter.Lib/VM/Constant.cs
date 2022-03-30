namespace Interpreter.Lib.VM
{
    public class Constant : IValue
    {
        private readonly object _value;
        private readonly string _representation;

        public Constant(object value, string representation)
        {
            _value = value;
            _representation = representation;
        }

        public object Get(Frame frame) => _value;

        public override string ToString() => _representation;
    }
}
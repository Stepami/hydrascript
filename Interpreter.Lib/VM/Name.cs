namespace Interpreter.Lib.VM
{
    public class Name : IValue
    {
        private readonly string _id;

        public Name(string id)
        {
            _id = id;
        }

        public object Get(Frame frame) => frame[_id];

        public override string ToString() => _id;
    }
}
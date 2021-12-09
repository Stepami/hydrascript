namespace Interpreter.Lib.VM
{
    public class Name : IValue
    {
        private readonly string id;

        public Name(string id)
        {
            this.id = id;
        }

        public object Get(Frame frame) => frame[id];

        public override string ToString() => id;
    }
}
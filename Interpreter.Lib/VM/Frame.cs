using System.Collections.Generic;

namespace Interpreter.Lib.VM
{
    public class Frame
    {
        private readonly Dictionary<string, object> variables = new();
        private readonly Frame parentFrame;

        public int ReturnAddress { get; }

        public Frame(int returnAddress = 0, Frame parentFrame = null)
        {
            ReturnAddress = returnAddress;
            this.parentFrame = parentFrame;
        }

        public object this[string id]
        {
            get => variables.ContainsKey(id)
                ? variables[id]
                : parentFrame?[id];
            set => variables[id] = value;
        }
    }
}
using System.Collections.Generic;

namespace Interpreter.Lib.VM
{
    public class Frame
    {
        private readonly Dictionary<string, object> _variables = new();
        private readonly Frame _parentFrame;

        public int ReturnAddress { get; }

        public Frame(int returnAddress = 0, Frame parentFrame = null)
        {
            ReturnAddress = returnAddress;
            _parentFrame = parentFrame;
        }

        public object this[string id]
        {
            get => _variables.ContainsKey(id)
                ? _variables[id]
                : _parentFrame?[id];
            set => _variables[id] = value;
        }
    }
}
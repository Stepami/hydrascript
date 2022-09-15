using System;
using System.Collections.Generic;

namespace Interpreter.Lib.BackEnd.Instructions
{
    public class CallFunction : Simple
    {
        private readonly FunctionInfo _function;
        private readonly int _numberOfArguments;
        
        public CallFunction(FunctionInfo function, int number, int numberOfArguments, string left = null) :
            base(left, (null, null), "Call ", number)
        {
            _function = function;
            _numberOfArguments = numberOfArguments + Convert.ToInt32(function.MethodOf != null);
        }

        public override int Jump() => _function.Location;

        public override int Execute(VirtualMachine vm)
        {
            var frame = new Frame(Number + 1, vm.Frames.Peek());

            var i = 0;
            var args = new List<(string Id, object Value)>();
            while (i < _numberOfArguments)
            {
                args.Add(vm.Arguments.Pop());
                frame[args[i].Id] = args[i].Value;
                i++;
            }

            if (_function.MethodOf != null)
            {
                var obj = (Dictionary<string, object>) frame[_function.MethodOf];
                foreach (var (key, value) in obj)
                {
                    frame[key] = value;
                }
            }

            vm.CallStack.Push(new Call(Number, _function, args, Left));
            vm.Frames.Push(frame);
            return _function.Location;
        }

        protected override string ToStringRepresentation() => Left == null
            ? $"Call {_function}, {_numberOfArguments}"
            : $"{Left} = Call {_function}, {_numberOfArguments}";
    }
}
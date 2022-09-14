using System.Linq;
using Interpreter.Lib.BackEnd.VM;

namespace Interpreter.Lib.BackEnd.Instructions
{
    public class CreateArray : Instruction
    {
        private readonly string _id;
        private readonly int _size;
        
        public CreateArray(int number, string id, int size) : base(number)
        {
            _id = id;
            _size = size;
        }

        public override int Execute(VirtualMachine vm)
        {
            var frame = vm.Frames.Peek();
            frame[_id] = new object[_size].ToList();
            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"array {_id} = [{_size}]";
    }
}
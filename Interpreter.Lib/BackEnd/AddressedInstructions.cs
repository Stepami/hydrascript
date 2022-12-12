using System.Collections.Generic;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.BackEnd
{
    public class AddressedInstructions
    {
        private readonly LinkedList<IAddress> _addresses;
        private readonly Dictionary<IAddress, LinkedListNode<IAddress>> _addressToNode;
        private readonly Dictionary<LinkedListNode<IAddress>, Instruction> _instructions;

        public AddressedInstructions()
        {
            _addresses = new();
            _addressToNode = new();
            _instructions = new();
        }

        public void Add(Instruction instruction, string label = null)
        {
            // сгенерировать адрес
            
            // вставить адрес в конец списка (список->новый адрес)
            
            // полученный узел списка связать с инструкцией
            _instructions.Add(null!, instruction);
            LinkedListNode<IAddress> node;
            LinkedList<IAddress> linkedList;
        }
    }
}
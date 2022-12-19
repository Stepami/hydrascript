using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.BackEnd;

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
        IAddress newAddress = label is null
            ? new SimpleAddress(_addresses.Count, instruction.GetHashCode())
            : new Label(label);
        
        var newNode = _addresses.AddLast(newAddress);
        
        _addressToNode.Add(newAddress, newNode);
        _instructions.Add(newNode, instruction);
    }

    public void AddRange(IEnumerable<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            var strAddress = instruction.Address.ToString();
            Add(instruction, strAddress!.StartsWith("address") ? null : strAddress);
        }
    }
}
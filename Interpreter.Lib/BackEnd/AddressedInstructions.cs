using System.Collections;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.BackEnd;

public class AddressedInstructions : IEnumerable<Instruction>
{
    private readonly LinkedList<IAddress> _addresses = new();
    private readonly Dictionary<IAddress, LinkedListNode<IAddress>> _addressToNode = new();
    private readonly Dictionary<LinkedListNode<IAddress>, Instruction> _instructions = new();

    public Instruction this[IAddress address] =>
        _instructions[_addressToNode[address]];

    public void Add(Instruction instruction, string label = null)
    {
        IAddress newAddress = label is null
            ? new SimpleAddress(_addresses.Count, instruction.GetHashCode())
            : new Label(label);

        var last = _addresses.Last;
        if (last is not null)
            last.Value.Next = newAddress;

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

    public IEnumerator<Instruction> GetEnumerator() =>
        _addresses.Select(address => this[address])
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}
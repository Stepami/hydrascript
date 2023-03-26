using System.Collections;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.BackEnd;

public class AddressedInstructions : IEnumerable<Instruction>
{
    private readonly LinkedList<IAddress> _addresses = new();
    private readonly Dictionary<IAddress, LinkedListNode<IAddress>> _addressToNode = new();
    private readonly Dictionary<LinkedListNode<IAddress>, Instruction> _instructions = new();

    public Instruction this[IAddress address]
    {
        get => _instructions[_addressToNode[address]];
        private set => _instructions[_addressToNode[address]] = value;
    }

    public IAddress Start =>
        _addresses.First?.Value;

    public void Add(Instruction instruction, string label = null)
    {
        IAddress newAddress = label is null
            ? new HashAddress(seed: instruction.GetHashCode())
            : new Label(label);
        instruction.Address = newAddress;

        AddWithAddress(instruction, newAddress);
    }

    public void Replace(Instruction old, Instruction @new)
    {
        var address = old.Address;
        @new.Address = address;

        this[address] = @new;
    }

    private void AddWithAddress(Instruction instruction, IAddress newAddress)
    {
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
            AddWithAddress(instruction, instruction.Address);
    }

    public void Remove(Instruction instruction)
    {
        var address = instruction.Address;
        var nodeToRemove = _addressToNode[address];
        
        var prev = nodeToRemove.Previous;
        if (prev is not null)
        {
            prev.Value.Next = nodeToRemove.Next?.Value;
        }

        _addressToNode.Remove(address);
        _instructions.Remove(nodeToRemove);
        _addresses.Remove(nodeToRemove);
    }

    public IEnumerator<Instruction> GetEnumerator() =>
        _addresses.Select(address => this[address])
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}
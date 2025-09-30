using System.Collections;
using Cysharp.Text;
using HydraScript.Domain.BackEnd.Impl.Addresses;

namespace HydraScript.Domain.BackEnd;

public class AddressedInstructions : IEnumerable<IExecutableInstruction>
{
    private readonly LinkedList<IAddress> _addresses = new();
    private readonly Dictionary<IAddress, LinkedListNode<IAddress>> _addressToNode = new();
    private readonly Dictionary<LinkedListNode<IAddress>, IExecutableInstruction> _instructions = new();

    public IExecutableInstruction this[IAddress address]
    {
        get => _instructions[_addressToNode[address]];
        private set => _instructions[_addressToNode[address]] = value;
    }

    public IAddress Start =>
        _addresses.First?.Value!;

    public IAddress End =>
        _addresses.Last?.Value!;

    public void Add(IExecutableInstruction instruction, string? label = null)
    {
        IAddress newAddress = label is null
            ? new HashAddress(seed: instruction.GetHashCode())
            : new Label(label);
        instruction.Address = newAddress;

        AddWithAddress(instruction, newAddress);
    }

    public void Replace(IExecutableInstruction old, IExecutableInstruction @new)
    {
        var address = old.Address;
        @new.Address = address;

        this[address] = @new;
    }

    private void AddWithAddress(IExecutableInstruction instruction, IAddress newAddress)
    {
        var last = _addresses.Last;
        if (last is not null)
            last.Value.Next = newAddress;

        var newNode = _addresses.AddLast(newAddress);
        
        _addressToNode.Add(newAddress, newNode);
        _instructions.Add(newNode, instruction);
    }

    public void AddRange(AddressedInstructions instructions)
    {
        for (var address = instructions.Start; address != null; address = address?.Next)
        {
            AddWithAddress(instructions[address], address);
        }
    }

    public void Remove(IExecutableInstruction instruction)
    {
        var address = instruction.Address;
        var nodeToRemove = _addressToNode[address];
        
        var prev = nodeToRemove.Previous;
        if (prev is not null)
        {
            prev.Value.Next = nodeToRemove.Next?.Value!;
        }

        _addressToNode.Remove(address);
        _instructions.Remove(nodeToRemove);
        _addresses.Remove(nodeToRemove);
    }

    public IEnumerator<IExecutableInstruction> GetEnumerator() =>
        _addresses.Select(address => this[address]).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString() =>
        ZString.Join<IExecutableInstruction>('\n', this);
}
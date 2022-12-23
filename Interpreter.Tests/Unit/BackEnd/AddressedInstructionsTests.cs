using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Xunit;

namespace Interpreter.Tests.Unit.BackEnd;

public class AddressedInstructionsTests
{
    private readonly AddressedInstructions _collection = new();
    
    [Fact]
    public void EnumerationPreservedAfterRemovalTest()
    {
        var instructions = new List<Instruction>
        {
            new AsString("s", new Constant(2, "2"), 0),
            new Print(1, new Name("s")),
            new Halt(2)
        };
        
        _collection.AddRange(instructions);
        
        _collection.Remove(instructions[1]);
        
        Assert.Same(instructions[2], _collection[instructions[0].Address.Next]);
    }
    
    [Fact]
    public void RemovalOfLastDoesNotThrow()
    {
        var instructions = new List<Instruction>
        {
            new AsString("s", new Constant(2, "2"), 0),
            new Halt(2)
        };
        
        instructions.ForEach(instruction => _collection.Add(instruction));
        
        Assert.Null(Record.Exception(() => _collection.Remove(instructions[1])));
        Assert.Null(instructions[0].Address.Next);
    }

    [Fact]
    public void GetEnumeratorTests()
    {
        _collection.Add(1.ToInstructionMock().Object);
        
        var collectionToAdd = new AddressedInstructions
        {
            2.ToInstructionMock().Object,
            3.ToInstructionMock().Object,
            4.ToInstructionMock().Object
        };
        
        _collection.AddRange(collectionToAdd);
        
        _collection.Add(5.ToInstructionMock().Object);

        Assert.Collection(
            _collection.Select(x => x.ToString()),
            x => Assert.Equal("1", x),
            x => Assert.Equal("2", x),
            x => Assert.Equal("3", x),
            x => Assert.Equal("4", x),
            x => Assert.Equal("5", x)
        );
    }
}
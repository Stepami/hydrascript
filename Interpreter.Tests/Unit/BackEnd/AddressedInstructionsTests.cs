using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Tests.Helpers;
using Xunit;

namespace Interpreter.Tests.Unit.BackEnd;

public class AddressedInstructionsTests
{
    [Fact]
    public void EnumerationPreservedAfterRemovalTest()
    {
        var instructions = new List<Instruction>
        {
            new AsString(new Constant(2, "2"))
            {
                Left = "s"
            },
            new Print(new Name("s")),
            new Halt()
        }.ToAddressedInstructions();

        instructions.Remove(instructions[instructions.Start.Next]);
        
        Assert.Same(instructions.Last(), instructions[instructions.Start.Next]);
    }
    
    [Fact]
    public void RemovalOfLastDoesNotThrow()
    {
        var instructions = new List<Instruction>
        {
            new AsString(new Constant(2, "2")),
            new Halt()
        }.ToAddressedInstructions();

        Assert.Null(Record.Exception(() => instructions.Remove(instructions.Last())));
        Assert.Null(instructions.Start.Next);
    }

    [Fact]
    public void GetEnumeratorTests()
    {
        AddressedInstructions collection = new();
        collection.Add(1.ToInstructionMock().Object);
        
        var collectionToAdd = new AddressedInstructions
        {
            2.ToInstructionMock().Object,
            3.ToInstructionMock().Object,
            4.ToInstructionMock().Object
        };
        
        collection.AddRange(collectionToAdd);
        
        collection.Add(5.ToInstructionMock().Object);

        Assert.Collection(
            collection.Select(x => x.ToString()),
            x => Assert.Equal("1", x),
            x => Assert.Equal("2", x),
            x => Assert.Equal("3", x),
            x => Assert.Equal("4", x),
            x => Assert.Equal("5", x)
        );
    }
}
using HydraScript.Lib.BackEnd;
using HydraScript.Lib.BackEnd.Addresses;
using HydraScript.Lib.BackEnd.Instructions;
using HydraScript.Lib.BackEnd.Instructions.WithAssignment;
using HydraScript.Lib.BackEnd.Values;
using HydraScript.Tests.Helpers;
using Xunit;

namespace HydraScript.Tests.Unit.BackEnd;

public class AddressedInstructionsTests
{
    [Fact]
    public void EnumerationPreservedAfterRemovalTest()
    {
        var instructions = new List<Instruction>
        {
            new AsString(new Constant(2))
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
    public void RemovalOfLastDoesNotThrowTest()
    {
        var instructions = new List<Instruction>
        {
            new AsString(new Constant(2)),
            new Halt()
        }.ToAddressedInstructions();

        Assert.Null(Record.Exception(() => instructions.Remove(instructions.Last())));
        Assert.Null(instructions.Start.Next);
    }

    [Fact]
    public void ReplacementPreservesOrderTest()
    {
        var instructions = new AddressedInstructions
        {
            new Simple("a", (new Constant(1), new Constant(2)), "-"),
            {
                new AsString(new Constant(true))
                    { Left = "s" },
                "as_str"
            },
            new Print(new Name("s"))
        };

        var old = instructions[new Label("as_str")];
        var @new = new AsString(new Name("a")) { Left = "s" };
        instructions.Replace(old, @new);

        var prev = instructions.First();
        var next = instructions.Last();

        Assert.Same(@new, instructions[prev.Address.Next]);
        Assert.Same(next, instructions[@new.Address.Next]);
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
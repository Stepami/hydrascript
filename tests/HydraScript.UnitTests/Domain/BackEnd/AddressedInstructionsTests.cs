using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using HydraScript.Domain.BackEnd.Impl.Instructions;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.UnitTests.Domain.BackEnd;

public class AddressedInstructionsTests
{
    [Fact]
    public void EnumerationPreservedAfterRemovalTest()
    {
        AddressedInstructions instructions =
        [
            new AsString(new Constant(2))
            {
                Left = new Name("s")
            },
            new Print(new Name("s")),
            new Halt()
        ];

        instructions.Remove(instructions[instructions.Start.Next!]);
        
        Assert.Same(instructions.Last(), instructions[instructions.Start.Next!]);
    }
    
    [Fact]
    public void RemovalOfLastDoesNotThrowTest()
    {
        AddressedInstructions instructions =
        [
            new AsString(new Constant(2)),
            new Halt()
        ];

        Assert.Null(Record.Exception(() => instructions.Remove(instructions.Last())));
        Assert.Null(instructions.Start.Next);
    }

    [Fact]
    public void ReplacementPreservesOrderTest()
    {
        var instructions = new AddressedInstructions
        {
            new Simple(new Name("a"), (new Constant(1), new Constant(2)), "-"),
            {
                new AsString(new Constant(true))
                    { Left = new Name("s") },
                "as_str"
            },
            new Print(new Name("s"))
        };

        var old = instructions[new Label("as_str")];
        var @new = new AsString(new Name("a")) { Left = new Name("s") };
        instructions.Replace(old, @new);

        var prev = instructions.First();
        var next = instructions.Last();

        Assert.Same(@new, instructions[prev.Address.Next!]);
        Assert.Same(next, instructions[@new.Address.Next!]);
    }
}
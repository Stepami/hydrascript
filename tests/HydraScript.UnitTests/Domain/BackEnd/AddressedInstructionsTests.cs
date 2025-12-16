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
                Left = new Name("s", Substitute.For<IFrame>())
            },
            new Print(new Name("s", Substitute.For<IFrame>())),
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
            new Simple(new Name("a", Substitute.For<IFrame>()), (new Constant(1), new Constant(2)), "-"),
            {
                new AsString(new Constant(true))
                    { Left = new Name("s", Substitute.For<IFrame>()) },
                "as_str"
            },
            new Print(new Name("s", Substitute.For<IFrame>())),
        };

        var old = instructions[new Label("as_str")];
        var @new = new AsString(new Name("a", Substitute.For<IFrame>()))
        {
            Left = new Name("s", Substitute.For<IFrame>()),
        };
        instructions.Replace(old, @new);

        var prev = instructions.First();
        var next = instructions.Last();

        Assert.Same(@new, instructions[prev.Address.Next!]);
        Assert.Same(next, instructions[@new.Address.Next!]);
    }
}
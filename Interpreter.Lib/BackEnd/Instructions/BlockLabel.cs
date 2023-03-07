using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions;

public abstract class BlockLabel : Instruction
{
    private readonly BlockPosition _blockPosition;
    private readonly BlockType _blockType;
    private readonly string _blockId;

    protected BlockLabel(BlockPosition blockPosition, BlockType blockType, string blockId)
    {
        _blockPosition = blockPosition;
        _blockType = blockType;
        _blockId = blockId;
    }

    public override IAddress Execute(VirtualMachine vm) =>
        Address.Next;

    protected override string ToStringInternal() =>
        $"{_blockPosition}{_blockType} {_blockId}";

    protected enum BlockPosition
    {
        Begin,
        End
    }
}

public enum BlockType
{
    Function,
    Loop,
    Condition
}

public class BeginBlock : BlockLabel
{
    public BeginBlock(BlockType blockType, string blockId) :
        base(BlockPosition.Begin, blockType, blockId) { }
}

public class EndBlock : BlockLabel
{
    public EndBlock(BlockType blockType, string blockId) :
        base(BlockPosition.End, blockType, blockId) { }
}
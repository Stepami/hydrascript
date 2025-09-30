namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public abstract class BlockLabel(
    BlockLabel.BlockPosition blockPosition,
    BlockType blockType,
    string blockId) : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams) => Address.Next;

    protected override string ToStringInternal() =>
        $"{blockPosition}{blockType} {blockId}";

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

public class BeginBlock(BlockType blockType, string blockId) :
    BlockLabel(BlockPosition.Begin, blockType, blockId);

public class EndBlock(BlockType blockType, string blockId) :
    BlockLabel(BlockPosition.End, blockType, blockId);
namespace HydraScript.Domain.BackEnd.Impl.Values;

public class Name(string id, IFrame frame) : IValue
{
    private readonly string _id = id;
    private IFrame _frame = frame;

    public object? Get() => _frame[_id];

    public void Set(object? value) => _frame[_id] = value;

    public void SetFrame(IFrame newFrame) => _frame = newFrame;

    public override string ToString() => _id;

    public bool Equals(IValue? other) =>
        other is Name that &&
        _id == that._id;

    internal static IFrame NullFrameInstance { get; } = new NullFrame();

    private sealed class NullFrame : IFrame
    {
        public object? this[string id]
        {
            get => null;
            set { }
        }
    }
}
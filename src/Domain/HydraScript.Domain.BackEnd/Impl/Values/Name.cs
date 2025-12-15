namespace HydraScript.Domain.BackEnd.Impl.Values;

public class Name(string id, IFrame frame) : IValue
{
    protected string Id { get; } = id;
    private IFrame _frame = frame;

    public object? Get() => _frame[Id];

    public void Set(object? value) => _frame[Id] = value;

    public void SetFrame(IFrame newFrame) => _frame = newFrame;

    public override string ToString() => Id;

    public bool Equals(IValue? other) =>
        other is Name that &&
        Id == that.Id;

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
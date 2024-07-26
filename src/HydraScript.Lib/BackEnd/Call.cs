using HydraScript.Lib.BackEnd.Impl.Addresses;

namespace HydraScript.Lib.BackEnd;

public record Call(
    IAddress From, FunctionInfo To, 
    List<CallArgument> Arguments,
    string? Where = null)
{
    public override string ToString() =>
        $"{From} => {To.Start}: {To.Id}({string.Join(", ", Arguments)})";
}

public record CallArgument(string Id, object? Value)
{
    public override string ToString() => $"{Id}: {Value}";
}

public record FunctionInfo(string Id)
{
    public Label Start => new($"Start_{this}");
    public Label End => new($"End_{this}");

    public override string ToString() => Id;
}
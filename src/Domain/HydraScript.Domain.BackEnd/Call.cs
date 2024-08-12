using HydraScript.Domain.BackEnd.Impl.Addresses;

namespace HydraScript.Domain.BackEnd;

public record Call(
    IAddress From,
    FunctionInfo To,
    string? Where = null)
{
    public override string ToString() =>
        $"{From}: {Where} => {To.Start}: {To.Id}";
}

public record FunctionInfo(string Id)
{
    public Label Start => new($"Start_{this}");
    public Label End => new($"End_{this}");

    public override string ToString() => Id;
}
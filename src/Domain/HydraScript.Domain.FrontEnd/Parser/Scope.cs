namespace HydraScript.Domain.FrontEnd.Parser;

public sealed record Scope
{
    private Scope(Guid id)
    {
        Id = id;
    }

    public Scope() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; }

    public Scope? OpenScope { get; private set; }

    public void AddOpenScope(Scope scope) =>
        OpenScope = scope;

    public override string ToString() => Id.ToString();

    public static readonly Scope Empty = new(Guid.Empty);
}
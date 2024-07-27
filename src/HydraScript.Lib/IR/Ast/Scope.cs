namespace HydraScript.Lib.IR.Ast;

public record Scope
{
    public Guid Id { get; } = Guid.NewGuid();
    public Scope? OpenScope { get; private set; }

    public void AddOpenScope(Scope scope) =>
        OpenScope = scope;

    public override string ToString() => Id.ToString();
}
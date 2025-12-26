namespace HydraScript.Application.StaticAnalysis;

public interface IJavaScriptTypesProvider
{
    public Type Number { get; }

    public Type Boolean { get; }

    public Type String { get; }

    public Type Null { get; }

    public Type Undefined { get; }

    public Type Void { get; }

    public IEnumerable<Type> GetDefaultTypes();

    public bool Contains(Type type);
}
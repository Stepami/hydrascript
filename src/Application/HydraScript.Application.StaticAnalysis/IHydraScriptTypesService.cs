namespace HydraScript.Application.StaticAnalysis;

public interface IHydraScriptTypesService
{
    public Type Number { get; }

    public Type Boolean { get; }

    public Type String { get; }

    public Type Undefined { get; }

    public Type Void { get; }

    public IEnumerable<Type> GetDefaultTypes();

    public bool Contains(Type type);

    public object? GetDefaultValueForType(Type type);

    public bool IsExplicitCastAllowed(Type from, Type to);
}
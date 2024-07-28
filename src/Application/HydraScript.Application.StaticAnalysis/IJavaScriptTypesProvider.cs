namespace HydraScript.Application.StaticAnalysis;

public interface IJavaScriptTypesProvider
{
    public IEnumerable<Type> GetDefaultTypes();

    public bool Contains(Type type);
}
namespace HydraScript.Application.StaticAnalysis.Services;

public interface IJavaScriptTypesProvider
{
    IEnumerable<Type> GetDefaultTypes();

    bool Contains(Type type);
}
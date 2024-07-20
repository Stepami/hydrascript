namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

public interface IJavaScriptTypesProvider
{
    IEnumerable<Type> GetDefaultTypes();
}
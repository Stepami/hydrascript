namespace Interpreter.Lib.IR.CheckSemantics.Visitors.TypeSystemLoader.Service;

public interface IJavaScriptTypesProvider
{
    IEnumerable<Type> GetDefaultTypes();
}
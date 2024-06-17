using HydraScript.Lib.IR.CheckSemantics.Types;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

internal class JavaScriptTypesProvider : IJavaScriptTypesProvider
{
    public IEnumerable<Type> GetDefaultTypes()
    {
        yield return "number";
        yield return "boolean";
        yield return "string";
        yield return new NullType();
        yield return "undefined";
        yield return "void";
    }
}
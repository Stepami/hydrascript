using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.Services.Impl;

internal class DefaultValueForTypeCalculator : IDefaultValueForTypeCalculator
{
    private readonly Type _boolean = "boolean";
    private readonly Type _number = "number";
    private readonly Type _string = "string";
    private readonly Type _void = "void";
    private readonly Type _null = new NullType();

    public object GetDefaultValueForType(Type type)
    {
        if (type.Equals(_boolean))
            return false;
        if (type.Equals(_number))
            return 0;
        if (type.Equals(_string))
            return string.Empty;
        if (type.Equals(_void))
            return new object();
        if (type.Equals(_null))
            return null;
        if (type is ArrayType)
            return new List<object>();
            
        return new object();
    }
}
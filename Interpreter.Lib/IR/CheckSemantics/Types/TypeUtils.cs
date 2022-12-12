namespace Interpreter.Lib.IR.CheckSemantics.Types;

public static class TypeUtils
{
    public static (
        Type Number, Type Boolean, Type String, Type Null, Type Undefined, Type Void
        ) JavaScriptTypes { get; } = (
        new Type("number"),
        new Type("boolean"),
        new Type("string"),
        new NullType(),
        new Type("undefined"),
        new Type("void")
    );

    public static object GetDefaultValue(Type type)
    {
        if (type.Equals(JavaScriptTypes.Boolean))
            return false;
        if (type.Equals(JavaScriptTypes.Number))
            return 0;
        if (type.Equals(JavaScriptTypes.String))
            return "";
        if (type.Equals(JavaScriptTypes.Void))
            return new Void();
        if (type.Equals(JavaScriptTypes.Null))
            return null;
        if (type is ArrayType)
            return new List<object>();
            
        return new Undefined();
    }

    public struct Undefined
    {
        public override string ToString() => JavaScriptTypes.Undefined.ToString();
    }

    private struct Void
    {
        public override string ToString() => JavaScriptTypes.Void.ToString();
    }
}
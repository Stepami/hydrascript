using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Utils
{
    public static class TypeUtils
    {
        public static (
            Type Number, Type Boolean, Type String, Type Null, Type Undefined, Type Object, Type Void
            ) JavaScriptTypes { get; } = (
            new Type("number", new Type("object")),
            new Type("boolean", new Type("object")),
            new Type("string", new Type("object")),
            new Type("null", new Type("object")),
            new Type("undefined"),
            new Type("object"),
            new Type("void")
        );

        public static Type GetJavaScriptType(string id)
        {
            return id switch
            {
                "number" => JavaScriptTypes.Number,
                "boolean" => JavaScriptTypes.Boolean,
                "string" => JavaScriptTypes.String,
                "object" => JavaScriptTypes.Object,
                _ => JavaScriptTypes.Undefined
            };
        }

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
            return new Undefined();
        }

        public struct Undefined
        {
            public override string ToString() => JavaScriptTypes.Undefined.ToString();
        }

        public struct Void
        {
            public override string ToString() => JavaScriptTypes.Void.ToString();
        }
    }
}
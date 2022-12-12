using System.Text;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Types.Visitors
{
    public class ObjectTypePrinter : 
        IVisitor<Type, string>,
        IVisitor<ObjectType, string>,
        IVisitor<ArrayType, string>,
        IVisitor<NullableType, string>,
        IVisitor<FunctionType, string>
    {
        private readonly ObjectType _reference;

        public ObjectTypePrinter(ObjectType reference) =>
            _reference = reference;
        
        public string Visit(Type visitable) =>
            visitable.ToString();

        public string Visit(ObjectType visitable)
        {
            var sb = new StringBuilder("{");
            foreach (var key in visitable.Keys)
            {
                var type = visitable[key];
                var prop = $"{key}: ";
                prop += type.Equals(_reference)
                    ? "@this"
                    : type.Recursive
                        ? key
                        : type.Accept(this);
                sb.Append(prop).Append(';');
            }

            return sb.Append('}').ToString();
        }

        public string Visit(ArrayType visitable)
        {
            var sb = new StringBuilder();
            sb.Append(visitable.Type.Equals(_reference)
                ? "@this"
                : visitable.Type.Accept(this)
            );

            return sb.Append("[]").ToString();
        }
        
        public string Visit(NullableType visitable)
        {
            var sb = new StringBuilder();
            sb.Append(visitable.Type.Equals(_reference)
                ? "@this"
                : visitable.Type.Accept(this)
            );

            return sb.Append('?').ToString();
        }

        public string Visit(FunctionType visitable)
        {
            var sb = new StringBuilder("(");
            sb.AppendJoin(", ", visitable.Arguments.Select(x => x.Equals(_reference)
                ? "@this"
                : x.Accept(this)
            )).Append(") => ");
            sb.Append(visitable.ReturnType.Equals(_reference)
                ? "@this"
                : visitable.ReturnType.Accept(this)
            );

            return sb.ToString();
        }
    }
}
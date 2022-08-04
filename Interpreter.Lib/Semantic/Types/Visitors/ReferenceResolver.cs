using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.Semantic.Types.Visitors
{
    public class ReferenceResolver :
        IVisitor<Type, Unit>,
        IVisitor<ArrayType, Unit>,
        IVisitor<FunctionType, Unit>,
        IVisitor<NullableType, Unit>, 
        IVisitor<ObjectType, Unit>
    {
        private readonly ObjectType _reference;
        private readonly string _refId;

        public ReferenceResolver(ObjectType reference, string refId)
        {
            _reference = reference;
            _refId = refId;
        }
        
        public Unit Visit(ObjectType visitable)
        {
            foreach (var key in visitable.Keys)
                if (_refId == visitable[key])
                    visitable[key] = _reference;
                else
                    visitable[key].Accept(this);
            return default;
        }

        public Unit Visit(Type visitable) => default;
        
        public Unit Visit(ArrayType visitable)
        {
            if (visitable.Type == _refId)
                visitable.Type = _reference;
            else
                visitable.Type.Accept(this);
            return default;
        }

        public Unit Visit(FunctionType visitable)
        {
            if (visitable.ReturnType == _refId)
                visitable.ReturnType = _reference;
            else
                visitable.ReturnType.Accept(this);

            for (var i = 0; i < visitable.Arguments.Count; i++)
            {
                var argType = visitable.Arguments[i];
                if (argType == _refId)
                    visitable.Arguments[i] = _reference;
                else
                    argType.Accept(this);
            }

            return default;
        }

        public Unit Visit(NullableType visitable)
        {
            if (visitable.Type == _refId)
                visitable.Type = _reference;
            else
                visitable.Type.Accept(this);
            return default;
        }
    }
}
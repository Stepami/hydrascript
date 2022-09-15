using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Types.Visitors
{
    public class ReferenceResolver :
        IVisitor<Type>,
        IVisitor<ArrayType>,
        IVisitor<FunctionType>,
        IVisitor<NullableType>, 
        IVisitor<ObjectType>
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
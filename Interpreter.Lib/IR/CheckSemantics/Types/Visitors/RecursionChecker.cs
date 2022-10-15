using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Types.Visitors
{
    public class RecursionChecker :
        IVisitor<Type, bool>,
        IVisitor<ObjectType, bool>,
        IVisitor<ArrayType, bool>,
        IVisitor<NullableType, bool>,
        IVisitor<FunctionType, bool>
    {
        private readonly string _reference;

        public RecursionChecker(string reference) =>
            _reference = reference;
        
        public bool Visit(Type visitable)
        {
            throw new System.NotImplementedException();
        }

        public bool Visit(ObjectType visitable)
        {
            throw new System.NotImplementedException();
        }

        public bool Visit(ArrayType visitable)
        {
            throw new System.NotImplementedException();
        }

        public bool Visit(NullableType visitable)
        {
            throw new System.NotImplementedException();
        }

        public bool Visit(FunctionType visitable)
        {
            throw new System.NotImplementedException();
        }
    }
}
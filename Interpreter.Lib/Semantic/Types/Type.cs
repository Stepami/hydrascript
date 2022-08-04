using Interpreter.Lib.Semantic.Types.Visitors;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.Semantic.Types
{
    public class Type : IVisitable<ReferenceResolver, Unit>
    {
        private readonly string _name;

        protected Type()
        {
        }

        public Type(string name) => _name = name;

        public virtual Unit Accept(ReferenceResolver visitor)
            => visitor.Visit(this);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var that = (Type) obj;
            return Equals(_name, that._name);
        }
        
        public override int GetHashCode() => 
            _name.GetHashCode();

        public override string ToString() => _name;

        public static implicit operator Type(string alias) => new(alias);

        public static bool operator ==(Type left, Type right) => Equals(left, right);

        public static bool operator !=(Type left, Type right) => !(left == right);
    }
}
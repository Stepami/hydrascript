namespace Interpreter.Lib.Semantic.Types
{
    public class Type
    {
        private readonly string _name;

        protected Type()
        {
        }

        public Type(string name) => _name = name;

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var that = (Type) obj;
            return Equals(_name, that._name);
        }
        
        public override int GetHashCode() => 
            _name.GetHashCode();

        public override string ToString() => _name;
    }
}
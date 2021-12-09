namespace Interpreter.Lib.Semantic.Types
{
    public class Type
    {
        private readonly string name;

        private readonly Type baseType;
        
        public bool CanBeNull { get; protected init; }

        public Type(string name, Type baseType = null)
        {
            this.name = name;
            this.baseType = baseType;
        }

        public bool SubTypeOf(Type other)
        {
            var type = baseType;
            while (type != null)
            {
                if (type.Equals(other))
                {
                    return true;
                }

                type = type.baseType;
            }

            return false;
        }
        
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var that = (Type) obj;
            return Equals(name, that.name);
        }
        
        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override string ToString()
        {
            return name;
        }
    }
}
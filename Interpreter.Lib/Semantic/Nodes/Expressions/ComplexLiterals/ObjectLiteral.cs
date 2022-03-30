using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.ComplexLiterals
{
    public class ObjectLiteral : Expression
    {
        private readonly List<Property> _properties;

        public ObjectLiteral(IEnumerable<Property> properties)
        {
            _properties = new List<Property>(properties);
            _properties.ForEach(prop => prop.Parent = this);
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() => 
            _properties.GetEnumerator();

        protected override string NodeRepresentation() => "{}";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}
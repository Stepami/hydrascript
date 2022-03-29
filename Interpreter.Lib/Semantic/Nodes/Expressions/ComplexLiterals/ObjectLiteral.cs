using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.ComplexLiterals
{
    public class ObjectLiteral : Expression
    {
        private readonly List<Property> properties;

        public ObjectLiteral(IEnumerable<Property> properties)
        {
            this.properties = new List<Property>(properties);
            this.properties.ForEach(prop => prop.Parent = this);
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() => 
            properties.GetEnumerator();

        protected override string NodeRepresentation() => "{}";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}
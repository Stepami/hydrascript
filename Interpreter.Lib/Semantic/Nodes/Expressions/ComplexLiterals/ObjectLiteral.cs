using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Types;

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

        internal override Type NodeCheck()
        {
            var propertyTypes = new List<PropertyType>();
            _properties.ForEach(prop =>
            {
                var propType = prop.Expression.NodeCheck();
                propertyTypes.Add(new PropertyType(prop.Id.Id, propType));
                prop.Id.SymbolTable.AddSymbol(new VariableSymbol(prop.Id.Id) {Type = propType});
            });
            return new ObjectType(propertyTypes);
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() => 
            _properties.GetEnumerator();

        protected override string NodeRepresentation() => "{}";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            // reserve frame for temp
            // foreach prop make assign temp.prop = expr
            throw new System.NotImplementedException();
        }
    }
}
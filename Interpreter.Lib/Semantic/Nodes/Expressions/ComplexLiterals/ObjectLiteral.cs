using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.VM.Values;

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
            var instructions = new List<Instruction>
            {
                new CreateObject(start, temp)
            };
            var i = 1;
            foreach (var (id, expr) in _properties)
            {
                if (expr is PrimaryExpression prim)
                {
                    instructions.Add(new DotAssignment(temp, (new Name(id), prim.ToValue()), start + i));
                    i++;
                }
                else
                {
                    var propInstructions = expr.ToInstructions(start + i, "_t" + (start + i));
                    i += propInstructions.Count;
                    var left = propInstructions.OfType<Simple>().Last().Left;
                    propInstructions.Add(new DotAssignment(temp, (new Name(id), new Name(left)), start + i));
                    i++;
                    instructions.AddRange(propInstructions);
                }
            }
            
            return instructions;
        }
    }
}
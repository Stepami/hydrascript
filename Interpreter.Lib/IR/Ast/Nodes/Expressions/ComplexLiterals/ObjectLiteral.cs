using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.ComplexLiterals;

public class ObjectLiteral : Expression
{
    private readonly List<Property> _properties;
    private readonly List<FunctionDeclaration> _methods;

    public ObjectLiteral(IEnumerable<Property> properties, IEnumerable<FunctionDeclaration> methods)
    {
        _properties = new List<Property>(properties);
        _properties.ForEach(prop => prop.Parent = this);

        _methods = new List<FunctionDeclaration>(methods);
        _methods.ForEach(m => m.Parent = this);
    }

    internal override Type NodeCheck()
    {
        var propertyTypes = new List<PropertyType>();
        _properties.ForEach(prop =>
        {
            var propType = prop.Expression.NodeCheck();
            propertyTypes.Add(new PropertyType(prop.Id.Id, propType));
            prop.Id.SymbolTable.AddSymbol(propType is ObjectType objectType
                ? new ObjectSymbol(prop.Id.Id, objectType) {Table = prop.Expression.SymbolTable}
                : new VariableSymbol(prop.Id.Id, propType)
            );
        });
        _methods.ForEach(m =>
        {
            var symbol = m.GetSymbol();
            propertyTypes.Add(new PropertyType(symbol.Id, symbol.Type));
        });
        var type = new ObjectType(propertyTypes);
        SymbolTable.AddSymbol(new VariableSymbol("this", type, true));
        return type;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        _properties.Concat<AbstractSyntaxTreeNode>(_methods).GetEnumerator();

    protected override string NodeRepresentation() => "{}";

    public override List<Instruction> ToInstructions(int start, string temp)
    {
        var instructions = new List<Instruction>();
        _methods.ForEach(method =>
        {
            var mInstructions = method.ToInstructions(start);
            instructions.AddRange(mInstructions);
            start += mInstructions.Count;
        });
            
        instructions.Add(new CreateObject(start, temp));
        var i = 1;
        foreach (var (id, expr) in _properties)
        {
            if (expr is PrimaryExpression prim)
            {
                instructions.Add(new DotAssignment(temp, (new Constant(id, @$"\""{id}\"""), prim.ToValue()), start + i));
                i++;
            }
            else
            {
                var propInstructions = expr.ToInstructions(start + i, "_t" + (start + i));
                i += propInstructions.Count;
                var left = propInstructions.OfType<Simple>().Last().Left;
                propInstructions.Add(new DotAssignment(temp, (new Constant(id, @$"\""{id}\"""), new Name(left)), start + i));
                i++;
                instructions.AddRange(propInstructions);
            }
        }

        return instructions;
    }
}
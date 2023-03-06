using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.ComplexLiterals;

public class ObjectLiteral : Expression
{
    public List<Property> Properties { get; }
    public List<FunctionDeclaration> Methods { get; }

    public ObjectLiteral(IEnumerable<Property> properties, IEnumerable<FunctionDeclaration> methods)
    {
        Properties = new List<Property>(properties);
        Properties.ForEach(prop => prop.Parent = this);

        Methods = new List<FunctionDeclaration>(methods);
        Methods.ForEach(m => m.Parent = this);
    }

    internal override Type NodeCheck()
    {
        var propertyTypes = new List<PropertyType>();
        Properties.ForEach(prop =>
        {
            var propType = prop.Expression.NodeCheck();
            propertyTypes.Add(new PropertyType(prop.Id.Id, propType));
            prop.Id.SymbolTable.AddSymbol(propType is ObjectType objectType
                ? new ObjectSymbol(prop.Id.Id, objectType) {Table = prop.Expression.SymbolTable}
                : new VariableSymbol(prop.Id.Id, propType)
            );
        });
        Methods.ForEach(m =>
        {
            var symbol = m.GetSymbol();
            propertyTypes.Add(new PropertyType(symbol.Id, symbol.Type));
        });
        var type = new ObjectType(propertyTypes);
        SymbolTable.AddSymbol(new VariableSymbol("this", type, true));
        return type;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Properties.Concat<AbstractSyntaxTreeNode>(Methods).GetEnumerator();

    protected override string NodeRepresentation() => "{}";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        throw new NotImplementedException();
}
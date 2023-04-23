using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

public class ObjectLiteral : ComplexLiteral
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
            propertyTypes.Add(new PropertyType(prop.Id.Name, propType));
            prop.Id.SymbolTable.AddSymbol(propType is ObjectType objectType
                ? new ObjectSymbol(prop.Id.Name, objectType) {Table = prop.Expression.SymbolTable}
                : new VariableSymbol(prop.Id.Name, propType)
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
        new InstructionProvider().Visit(this);

    public override Unit Accept(SymbolTableBuilder visitor) =>
        visitor.Visit(this);
}
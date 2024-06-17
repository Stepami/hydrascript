using HydraScript.Lib.BackEnd.Values;
using HydraScript.Lib.FrontEnd.GetTokens.Data;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

public class Literal : PrimaryExpression
{
    private readonly TypeValue _type;
    private readonly object _value;
    private readonly string _label;

    public Literal(
        TypeValue type,
        object value,
        Segment segment,
        string label = null)
    {
        _type = type;
        _label = label ?? value.ToString();
        _value = value;
        Segment = segment;
    }

    protected override string NodeRepresentation() => _label;

    public override IValue ToValue() =>
        new Constant(_value, _label);

    public override Type Accept(SemanticChecker visitor) =>
        _type.BuildType(Parent.SymbolTable);
}
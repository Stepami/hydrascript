using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;

public class Literal : PrimaryExpression
{
    private readonly Type _type;
    private readonly object _value;
    private readonly string _label;

    public Literal(Type type, object value, Segment segment = null, string label = null)
    {
        _type = type;
        _label = label ?? value.ToString();
        _value = value;
        Segment = segment;
    }

    internal override Type NodeCheck() => _type;

    protected override string NodeRepresentation() => _label;

    public override IValue ToValue() => new Constant(_value, _label);
}
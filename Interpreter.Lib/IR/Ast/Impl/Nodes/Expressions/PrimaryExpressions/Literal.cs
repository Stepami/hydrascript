using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

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

    protected override string NodeRepresentation() => _label;

    public override IValue ToValue() =>
        new Constant(_value, _label);

    public override Type Accept(SemanticChecker visitor) => _type;
}
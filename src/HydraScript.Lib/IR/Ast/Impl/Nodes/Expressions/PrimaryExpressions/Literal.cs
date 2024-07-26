using HydraScript.Lib.BackEnd.Values;
using HydraScript.Lib.FrontEnd.GetTokens.Data;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class Literal : PrimaryExpression
{
    public TypeValue Type { get; }
    private readonly object? _value;
    private readonly string _label;

    public Literal(
        TypeValue type,
        object? value,
        Segment segment,
        string? label = null)
    {
        Type = type;
        _label = (label ?? value?.ToString())!;
        _value = value;
        Segment = segment;
    }

    protected override string NodeRepresentation() => _label;

    public override IValue ToValue() =>
        new Constant(_value, _label);
}
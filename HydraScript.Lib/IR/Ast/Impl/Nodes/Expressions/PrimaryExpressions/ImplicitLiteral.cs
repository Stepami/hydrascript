using HydraScript.Lib.BackEnd.Values;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

public class ImplicitLiteral : PrimaryExpression
{
    public TypeValue TypeValue { get; }
    public object ComputedDefaultValue { private get; set; }

    public ImplicitLiteral(TypeValue typeValue) =>
        TypeValue = typeValue;

    protected override string NodeRepresentation() =>
        TypeValue.ToString();

    public override IValue ToValue() =>
        new Constant(
            ComputedDefaultValue,
            ComputedDefaultValue is null
                ? "null"
                : ComputedDefaultValue.ToString());
    
    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);
}
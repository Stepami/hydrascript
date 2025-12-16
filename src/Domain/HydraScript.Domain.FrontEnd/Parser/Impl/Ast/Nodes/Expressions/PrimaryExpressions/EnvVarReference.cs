using Cysharp.Text;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class EnvVarReference(string name) : IdentifierReference(name)
{
    protected override string NodeRepresentation() => ZString.Concat('$', Name);

    public override ValueDto ToValueDto() =>
        ValueDto.EnvDto(Name);
}
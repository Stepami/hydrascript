using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.CheckSemantics.Exceptions;
using HydraScript.Lib.IR.CheckSemantics.Types;
using HydraScript.Lib.IR.CheckSemantics.Variables;
using HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

public abstract record TypeValue
{
    public abstract Type BuildType(SymbolTable symbolTable);
}

public record TypeIdentValue(
    IdentifierReference TypeId
) : TypeValue
{
    public override Type BuildType(SymbolTable symbolTable) =>
        symbolTable.FindSymbol<TypeSymbol>(TypeId)?.Type ??
        throw new UnknownIdentifierReference(TypeId);

    public override string ToString() => TypeId;
}

public record ArrayTypeValue(
    TypeValue TypeValue
) : TypeValue
{
    public override Type BuildType(SymbolTable symbolTable) =>
        new ArrayType(TypeValue.BuildType(symbolTable));

    public override string ToString() => $"{TypeValue}[]";
}

public record NullableTypeValue(
    TypeValue TypeValue
) : TypeValue
{
    public override Type BuildType(SymbolTable symbolTable) =>
        new NullableType(TypeValue.BuildType(symbolTable));

    public override string ToString() => $"{TypeValue}?";
}

public record PropertyTypeValue(
    string Key,
    TypeValue TypeValue)
{
    public override string ToString() =>
        $"{Key}: {TypeValue}";
};

public record ObjectTypeValue(
    IEnumerable<PropertyTypeValue> Properties
) : TypeValue
{
    public override Type BuildType(SymbolTable symbolTable) =>
        new ObjectType(
            Properties.Select(x => new PropertyType(
                Id: x.Key,
                x.TypeValue.BuildType(symbolTable))));

    public override string ToString() =>
        $"{{{string.Join(';', Properties)}}}";
}
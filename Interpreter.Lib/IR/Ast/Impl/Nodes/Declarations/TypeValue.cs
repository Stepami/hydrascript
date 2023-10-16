using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;

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

public record FunctionTypeValue(
    TypeValue ReturnTypeValue,
    IEnumerable<TypeValue> Arguments
) : TypeValue
{
    public override Type BuildType(SymbolTable symbolTable) =>
        new FunctionType(
            ReturnTypeValue.BuildType(symbolTable),
            Arguments.Select(x => x.BuildType(symbolTable)));

    public override string ToString() =>
        $"({string.Join(',', Arguments)}) => {ReturnTypeValue}";
}
using HydraScript.Application.StaticAnalysis.Exceptions;
using HydraScript.Application.StaticAnalysis.Services;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis;

public class TypeBuilder : VisitorBase<TypeValue, Type>,
    IVisitor<TypeIdentValue, Type>,
    IVisitor<ArrayTypeValue, ArrayType>,
    IVisitor<NullableTypeValue, NullableType>,
    IVisitor<ObjectTypeValue, ObjectType>
{
    private readonly ISymbolTableStorage _symbolTables;

    public TypeBuilder(ISymbolTableStorage symbolTables)
    {
        _symbolTables = symbolTables;
    }

    public Type Visit(TypeIdentValue visitable) =>
        _symbolTables[visitable.Scope].FindSymbol<TypeSymbol>(visitable.TypeId)?.Type ??
        throw new UnknownIdentifierReference(visitable.TypeId);

    public ArrayType Visit(ArrayTypeValue visitable)
    {
        visitable.TypeValue.Scope = visitable.Scope;
        return new ArrayType(visitable.TypeValue.Accept(This));
    }

    public NullableType Visit(NullableTypeValue visitable)
    {
        visitable.TypeValue.Scope = visitable.Scope;
        return new NullableType(visitable.TypeValue.Accept(This));
    }

    public ObjectType Visit(ObjectTypeValue visitable) =>
        new(visitable.Properties
            .Select(x =>
            {
                x.TypeValue.Scope = visitable.Scope;
                return new PropertyType(
                    Id: x.Key,
                    x.TypeValue.Accept(This));
            }));
}
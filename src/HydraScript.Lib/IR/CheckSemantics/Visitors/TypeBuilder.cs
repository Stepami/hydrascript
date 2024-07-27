using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Exceptions;
using HydraScript.Lib.IR.CheckSemantics.Types;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl.Symbols;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors;

public class TypeBuilder : VisitorBase<TypeValue, Type>,
    IVisitor<TypeIdentValue, Type>,
    IVisitor<ArrayTypeValue, ArrayType>,
    IVisitor<NullableTypeValue, NullableType>,
    IVisitor<ObjectTypeValue, ObjectType>
{
    public Type Visit(TypeIdentValue visitable) =>
        visitable.Scope.FindSymbol<TypeSymbol>(visitable.TypeId)?.Type ??
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
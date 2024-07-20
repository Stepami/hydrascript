using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

public interface ITypeDeclarationsResolver
{
    void Store(TypeDeclaration declaration);

    void Resolve();
}
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

namespace HydraScript.Application.StaticAnalysis;

public interface ITypeDeclarationsResolver
{
    public void Store(TypeDeclaration declaration);

    public void Resolve();
}
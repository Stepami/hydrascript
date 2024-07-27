using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

namespace HydraScript.Application.StaticAnalysis.Services;

public interface ITypeDeclarationsResolver
{
    void Store(TypeDeclaration declaration);

    void Resolve();
}
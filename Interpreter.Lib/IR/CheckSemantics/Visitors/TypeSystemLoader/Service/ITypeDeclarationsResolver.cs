using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.TypeSystemLoader.Service;

public interface ITypeDeclarationsResolver
{
    void Store(TypeDeclaration declaration);

    void Resolve();
}
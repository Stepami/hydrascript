using Interpreter.Lib.IR.Ast;
using Interpreter.Lib.IR.Ast.Impl.Nodes;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Interpreter.Lib.IR.CheckSemantics.Visitors.TypeSystemLoader.Service;
using Visitor.NET;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.TypeSystemLoader;

public class TypeSystemLoader :
    IVisitor<ScriptBody>,
    IVisitor<AbstractSyntaxTreeNode>,
    IVisitor<TypeDeclaration>
{
    private readonly ITypeDeclarationsResolver _resolver;

    public TypeSystemLoader(ITypeDeclarationsResolver resolver) =>
        _resolver = resolver;

    public Unit Visit(ScriptBody visitable)
    {
        visitable.StatementList.ForEach(item => item.Accept(this));
        _resolver.Resolve();
        return default;
    }

    public Unit Visit(AbstractSyntaxTreeNode visitable)
    {
        foreach (var child in visitable)
            child.Accept(this);

        return default;
    }

    public Unit Visit(TypeDeclaration visitable)
    {
        visitable.SymbolTable.AddSymbol(
            new TypeSymbol(
                visitable.TypeId,
                visitable.TypeId));

        _resolver.Store(visitable);
        return default;
    }
}
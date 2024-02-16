using Interpreter.Lib.IR.Ast;
using Interpreter.Lib.IR.Ast.Impl.Nodes;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Interpreter.Lib.IR.CheckSemantics.Visitors.Services;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors;

public class TypeSystemLoader :
    IVisitor<ScriptBody>,
    IVisitor<AbstractSyntaxTreeNode>,
    IVisitor<TypeDeclaration>
{
    private readonly ITypeDeclarationsResolver _resolver;
    private readonly ISet<Type> _defaultTypes;

    public TypeSystemLoader(
        ITypeDeclarationsResolver resolver,
        IJavaScriptTypesProvider provider)
    {
        _resolver = resolver;
        _defaultTypes = provider.GetDefaultTypes().ToHashSet();
    }

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
        if (visitable.SymbolTable.ContainsSymbol(visitable.TypeId) ||
            _defaultTypes.Contains(visitable.TypeId.Name))
            throw new DeclarationAlreadyExists(visitable.TypeId);

        visitable.SymbolTable.AddSymbol(
            new TypeSymbol(
                visitable.TypeId.Name,
                visitable.TypeId));

        _resolver.Store(visitable);
        return default;
    }
}
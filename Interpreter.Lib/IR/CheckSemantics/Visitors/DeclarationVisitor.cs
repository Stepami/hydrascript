using Interpreter.Lib.IR.Ast;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Visitor.NET;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors;

public class DeclarationVisitor : 
    IVisitor<AbstractSyntaxTreeNode>,
    IVisitor<TypeDeclaration>,
    IVisitor<LexicalDeclaration>,
    IVisitor<FunctionDeclaration>
{
    public Unit Visit(AbstractSyntaxTreeNode visitable)
    {
        foreach (var child in visitable)
            child.Accept(this);

        return default;
    }

    public Unit Visit(TypeDeclaration visitable)
    {
        visitable.SymbolTable.AddSymbol(new TypeSymbol(visitable.TypeValue, visitable.TypeId));
        return default;
    }

    public Unit Visit(LexicalDeclaration visitable)
    {
        throw new NotImplementedException();
    }

    public Unit Visit(FunctionDeclaration visitable)
    {
        //visitable.Parent.SymbolTable.AddSymbol(visitable.GetSymbol());
        throw new NotImplementedException();
    }
}
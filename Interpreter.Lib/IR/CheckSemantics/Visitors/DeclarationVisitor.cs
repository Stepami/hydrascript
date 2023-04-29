using Interpreter.Lib.IR.Ast;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Visitor.NET.Lib.Core;

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
        throw new NotImplementedException();
    }

    public Unit Visit(LexicalDeclaration visitable)
    {
        throw new NotImplementedException();
    }

    public Unit Visit(FunctionDeclaration visitable)
    {
        throw new NotImplementedException();
    }
}
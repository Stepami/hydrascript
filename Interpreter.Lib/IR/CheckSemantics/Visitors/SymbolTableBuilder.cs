using Interpreter.Lib.IR.Ast;
using Interpreter.Lib.IR.Ast.Impl.Nodes;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;
using Interpreter.Lib.IR.CheckSemantics.Initializer;
using Interpreter.Lib.IR.CheckSemantics.Variables;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors;

public class SymbolTableBuilder :
    IVisitor<AbstractSyntaxTreeNode>,
    IVisitor<ScriptBody>,
    IVisitor<FunctionDeclaration>,
    IVisitor<BlockStatement>,
    IVisitor<ObjectLiteral>
{
    private readonly ISymbolTableInitializer _initializer;

    public SymbolTableBuilder(ISymbolTableInitializer initializer) =>
        _initializer = initializer;

    public Unit Visit(AbstractSyntaxTreeNode visitable) =>
        _initializer.InitThroughParent(visitable);

    public Unit Visit(ScriptBody visitable)
    {
        visitable.SymbolTable = SymbolTableUtils.GetStandardLibrary();
        return default;
    }

    public Unit Visit(FunctionDeclaration visitable) =>
        _initializer.InitWithNewScope(visitable);
    
    public Unit Visit(BlockStatement visitable) =>
        _initializer.InitWithNewScope(visitable);

    public Unit Visit(ObjectLiteral visitable) =>
        _initializer.InitWithNewScope(visitable);
}
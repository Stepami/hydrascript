using Interpreter.Lib.IR.Ast;
using Interpreter.Lib.IR.Ast.Impl.Nodes;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;
using Interpreter.Lib.IR.CheckSemantics.Variables;
using Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer.Service;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer;

public class SymbolTableInitializer :
    IVisitor<AbstractSyntaxTreeNode>,
    IVisitor<ScriptBody>,
    IVisitor<FunctionDeclaration>,
    IVisitor<BlockStatement>,
    IVisitor<ObjectLiteral>
{
    private readonly ISymbolTableInitializerService _initializerService;

    public SymbolTableInitializer(ISymbolTableInitializerService initializerService) =>
        _initializerService = initializerService;

    public Unit Visit(AbstractSyntaxTreeNode visitable) =>
        _initializerService.InitThroughParent(visitable);

    public Unit Visit(ScriptBody visitable)
    {
        visitable.SymbolTable = SymbolTableUtils.GetStandardLibrary();
        return default;
    }

    public Unit Visit(FunctionDeclaration visitable) =>
        _initializerService.InitWithNewScope(visitable);
    
    public Unit Visit(BlockStatement visitable) =>
        _initializerService.InitWithNewScope(visitable);

    public Unit Visit(ObjectLiteral visitable) =>
        _initializerService.InitWithNewScope(visitable);
}
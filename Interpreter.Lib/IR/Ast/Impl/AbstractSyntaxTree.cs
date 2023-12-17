using System.Text;
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors.Services.Impl;

namespace Interpreter.Lib.IR.Ast.Impl;

public class AbstractSyntaxTree : IAbstractSyntaxTree
{
    private readonly AbstractSyntaxTreeNode _root;
    
    private readonly SymbolTableInitializer _symbolTableInitializer;
    private readonly TypeSystemLoader _typeSystemLoader;
    private readonly DeclarationVisitor _declarationVisitor;
    
    private readonly SemanticChecker _semanticChecker;
    private readonly InstructionProvider _instructionProvider;

    public AbstractSyntaxTree(AbstractSyntaxTreeNode root)
    {
        _root = root;
        var storage = new FunctionWithUndefinedReturnStorage();
        
        _symbolTableInitializer = new SymbolTableInitializer(
            new SymbolTableInitializerService(),
            new StandardLibraryProvider(
                new JavaScriptTypesProvider()));
        _typeSystemLoader = new TypeSystemLoader(
            new TypeDeclarationsResolver(
                new JavaScriptTypesProvider()),
            new JavaScriptTypesProvider());
        _declarationVisitor = new DeclarationVisitor(storage);
        
        _semanticChecker = new SemanticChecker(
            new DefaultValueForTypeCalculator(),
            storage);
        _instructionProvider = new InstructionProvider();
    }

    public AddressedInstructions GetInstructions()
    {
        _root.Accept(_symbolTableInitializer);
        _root.Accept(_typeSystemLoader);
        _root.Accept(_declarationVisitor);
        
        _root.Accept(_semanticChecker);
        return _root.Accept(_instructionProvider);
    }

    public override string ToString()
    {
        var tree = new StringBuilder("digraph ast {\n");
        _root.GetAllNodes().ForEach(node =>
        {
            tree.Append('\t').Append(node).Append('\n');
            node.ToList().ForEach(child => tree.Append($"\t{node.GetHashCode()}->{child.GetHashCode()}\n"));
        });
        return tree.Append("}\n").ToString();
    }
}
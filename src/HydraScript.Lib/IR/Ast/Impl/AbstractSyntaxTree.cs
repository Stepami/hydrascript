using System.Text;
using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

namespace HydraScript.Lib.IR.Ast.Impl;

public class AbstractSyntaxTree : IAbstractSyntaxTree
{
    private readonly AbstractSyntaxTreeNode _root;
    
    private readonly IVisitor<AbstractSyntaxTreeNode> _symbolTableInitializer;
    private readonly IVisitor<AbstractSyntaxTreeNode> _typeSystemLoader;
    private readonly IVisitor<AbstractSyntaxTreeNode> _declarationVisitor;
    
    private readonly IVisitor<AbstractSyntaxTreeNode, Type> _semanticChecker;
    private readonly IVisitor<AbstractSyntaxTreeNode, AddressedInstructions> _instructionProvider;

    public AbstractSyntaxTree(AbstractSyntaxTreeNode root)
    {
        _root = root;
        var functionStorage = new FunctionWithUndefinedReturnStorage();
        var methodStorage = new MethodStorage();
        
        _symbolTableInitializer = new SymbolTableInitializer(
            new SymbolTableInitializerService(),
            new StandardLibraryProvider(
                new JavaScriptTypesProvider()));
        _typeSystemLoader = new TypeSystemLoader(
            new TypeDeclarationsResolver(
                new JavaScriptTypesProvider()),
            new JavaScriptTypesProvider());
        _declarationVisitor = new DeclarationVisitor(functionStorage, methodStorage);
        
        _semanticChecker = new SemanticChecker(
            new DefaultValueForTypeCalculator(),
            functionStorage,
            methodStorage);
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
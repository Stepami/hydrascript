using System.Text;
using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

namespace HydraScript.Lib.IR.Ast.Impl;

public class AbstractSyntaxTree : IAbstractSyntaxTree
{
    private readonly IVisitor<IAbstractSyntaxTreeNode> _symbolTableInitializer;
    private readonly IVisitor<IAbstractSyntaxTreeNode> _typeSystemLoader;
    private readonly IVisitor<IAbstractSyntaxTreeNode> _declarationVisitor;

    private readonly IVisitor<IAbstractSyntaxTreeNode, Type> _semanticChecker;
    private readonly IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions> _instructionProvider;

    public IAbstractSyntaxTreeNode Root { get; }

    public AbstractSyntaxTree(IAbstractSyntaxTreeNode root)
    {
        Root = root;
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
        Root.Accept(_symbolTableInitializer);
        Root.Accept(_typeSystemLoader);
        Root.Accept(_declarationVisitor);
        
        Root.Accept(_semanticChecker);
        return Root.Accept(_instructionProvider);
    }

    public override string ToString()
    {
        var tree = new StringBuilder("digraph ast {\n");
        var nodes = Root.GetAllNodes();
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            tree.Append('\t').Append(node).Append('\n');
            for (var j = 0; j < node.Count; j++)
            {
                var child = node[j];
                tree.Append($"\t{node.GetHashCode()}->{child.GetHashCode()}\n");
            }
        }
        return tree.Append("}\n").ToString();
    }
}
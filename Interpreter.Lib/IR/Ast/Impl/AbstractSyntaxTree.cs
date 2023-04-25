using System.Text;
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer;
using Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer.Service.Impl;

namespace Interpreter.Lib.IR.Ast.Impl;

public class AbstractSyntaxTree : IAbstractSyntaxTree
{
    private readonly AbstractSyntaxTreeNode _root;
    private readonly SymbolTableInitializer _symbolTableInitializer;
    private readonly SemanticChecker _semanticChecker;
    private readonly InstructionProvider _instructionProvider;

    public AbstractSyntaxTree(AbstractSyntaxTreeNode root)
    {
        _root = root;
        _symbolTableInitializer = new SymbolTableInitializer(new SymbolTableInitializerService());
        _semanticChecker = new();
        _instructionProvider = new();
    }

    private void Check() =>
        GetAllNodes().ToList().ForEach(node => node.SemanticCheck());

    private IEnumerable<AbstractSyntaxTreeNode> GetAllNodes() =>
        _root.GetAllNodes();

    public AddressedInstructions GetInstructions()
    {
        _root.Accept(_symbolTableInitializer);
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
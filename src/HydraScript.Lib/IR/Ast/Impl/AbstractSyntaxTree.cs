using System.Text;

namespace HydraScript.Lib.IR.Ast.Impl;

internal class AbstractSyntaxTree(IAbstractSyntaxTreeNode root) : IAbstractSyntaxTree
{
    public IAbstractSyntaxTreeNode Root { get; } = root;

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
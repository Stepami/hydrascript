using Cysharp.Text;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast;

internal class AbstractSyntaxTree(IAbstractSyntaxTreeNode root) : IAbstractSyntaxTree
{
    public IAbstractSyntaxTreeNode Root { get; } = root;

    public override string ToString()
    {
        using var tree = ZString.CreateStringBuilder();
        tree.Append("digraph ast {\n");
        var nodes = Root.GetAllNodes();
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            tree.AppendFormat("\t{0}\n", node);
            for (var j = 0; j < node.Count; j++)
            {
                var child = node[j];
                tree.AppendFormat("\t{0}->{1}\n", node.GetHashCode(), child.GetHashCode());
            }
        }
        tree.Append("}\n");
        return tree.ToString();
    }
}
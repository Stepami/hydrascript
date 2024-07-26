using System.Collections;
using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.Ast;

public abstract class AbstractSyntaxTreeNode :
    IReadOnlyList<AbstractSyntaxTreeNode>,
    IVisitable<AbstractSyntaxTreeNode>
{
    public AbstractSyntaxTreeNode Parent { get; set; } = default!;

    protected virtual bool IsRoot => false;

    public SymbolTable SymbolTable { get; set; } = default!;

    public string Segment { get; init; } = string.Empty;

    protected virtual IReadOnlyList<AbstractSyntaxTreeNode> Children { get; } = [];

    public IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Children.ToList().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public int Count => Children.Count;

    public AbstractSyntaxTreeNode this[int index] =>
        Children[index];

    internal List<AbstractSyntaxTreeNode> GetAllNodes()
    {
        List<AbstractSyntaxTreeNode> result = [this];
        for (var index = 0; index < Children.Count; index++)
            result.AddRange(Children[index].GetAllNodes());

        return result;
    }

    public bool ChildOf<T>() where T : AbstractSyntaxTreeNode
    {
        var parent = Parent;
        while (!parent.IsRoot)
        {
            if (parent is T)
            {
                return true;
            }

            parent = parent.Parent;
        }

        return false;
    }

    public virtual TReturn Accept<TReturn>(IVisitor<AbstractSyntaxTreeNode, TReturn> visitor) =>
        visitor.DefaultVisit;

    protected abstract string NodeRepresentation();

    public override string ToString() =>
        $"{GetHashCode()} [label=\"{NodeRepresentation()}\"]";
}
using System.Collections;
using HydraScript.Lib.FrontEnd.GetTokens.Data;
using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.Ast;

public abstract class AbstractSyntaxTreeNode :
    IEnumerable<AbstractSyntaxTreeNode>,
    IVisitable<AbstractSyntaxTreeNode>
{
    public AbstractSyntaxTreeNode Parent { get; set; } = default!;

    protected virtual bool IsRoot => false;

    public SymbolTable SymbolTable { get; set; } = default!;

    public Segment Segment { get; init; } = default!;

    internal List<AbstractSyntaxTreeNode> GetAllNodes()
    {
        var result = new List<AbstractSyntaxTreeNode>
        {
            this
        };
        foreach (var child in this)
        {
            result.AddRange(child.GetAllNodes());
        }

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

    public abstract IEnumerator<AbstractSyntaxTreeNode> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public virtual TReturn Accept<TReturn>(IVisitor<AbstractSyntaxTreeNode, TReturn> visitor) =>
        visitor.DefaultVisit;

    protected abstract string NodeRepresentation();

    public override string ToString() =>
        $"{GetHashCode()} [label=\"{NodeRepresentation()}\"]";
}
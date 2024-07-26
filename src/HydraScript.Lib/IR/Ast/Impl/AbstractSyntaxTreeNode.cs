using System.Collections;
using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.Ast.Impl;

public abstract class AbstractSyntaxTreeNode : IAbstractSyntaxTreeNode
{
    public IAbstractSyntaxTreeNode Parent { get; set; } = default!;

    public SymbolTable SymbolTable { get; set; } = default!;

    public string Segment { get; init; } = string.Empty;

    protected virtual IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; } = [];

    public IEnumerator<IAbstractSyntaxTreeNode> GetEnumerator() =>
        Children.ToList().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public int Count => Children.Count;

    public IAbstractSyntaxTreeNode this[int index] =>
        Children[index];

    public IReadOnlyList<IAbstractSyntaxTreeNode> GetAllNodes()
    {
        List<IAbstractSyntaxTreeNode> result = [this];
        for (var index = 0; index < Children.Count; index++)
            result.AddRange(Children[index].GetAllNodes());

        return result;
    }

    public bool ChildOf<T>() where T : IAbstractSyntaxTreeNode
    {
        var parent = Parent;
        while (parent != default!)
        {
            if (parent is T)
            {
                return true;
            }

            parent = parent.Parent;
        }

        return false;
    }

    public virtual TReturn Accept<TReturn>(IVisitor<IAbstractSyntaxTreeNode, TReturn> visitor) =>
        visitor.DefaultVisit;

    protected abstract string NodeRepresentation();
    public override string ToString() =>
        $"{GetHashCode()} [label=\"{NodeRepresentation()}\"]";
}
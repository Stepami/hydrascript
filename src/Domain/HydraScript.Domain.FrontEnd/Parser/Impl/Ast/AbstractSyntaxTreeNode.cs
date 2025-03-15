using System.Collections;
using System.Runtime.CompilerServices;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast;

public abstract class AbstractSyntaxTreeNode : IAbstractSyntaxTreeNode
{
    public IAbstractSyntaxTreeNode Parent { get; set; } = default!;

    public Scope Scope { get; protected set; } = default!;

    /// <summary>Базовая стратегия - инициализация через родительский узел</summary>
    /// <param name="scope">Обязательно <c>null</c></param>
    public virtual void InitScope(Scope? scope = null)
    {
        if (scope is not null)
            throw new ArgumentException("'scope' must be null");
        Scope = Parent.Scope;
    }

    public string Segment { get; init; } = string.Empty;

    protected virtual IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; } = [];

    public IEnumerator<IAbstractSyntaxTreeNode> GetEnumerator() =>
        Children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        Children.GetEnumerator();

    public int Count => Children.Count;

    public IAbstractSyntaxTreeNode this[int index] =>
        Children[index];

    public IReadOnlyList<IAbstractSyntaxTreeNode> GetAllNodes() => new TraverseEnumerator(this).ToList();


    /// <summary>
    /// Метод возвращает <c>true</c>, если узел - потомок заданного типа и выполняется заданное условие.<br/>
    /// В случае, когда условие не задано, проверяется просто соответствие типов.
    /// </summary>
    /// <param name="condition">Условие для родителя</param>
    /// <typeparam name="T">Проверяемый тип родителя</typeparam>
    public bool ChildOf<T>(Predicate<T>? condition = null) where T : IAbstractSyntaxTreeNode
    {
        var parent = Parent;
        while (parent != default!)
        {
            if (parent is T node)
            {
                return condition is not null
                    ? condition(node)
                    : true;
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
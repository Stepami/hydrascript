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
        Children.ToList().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public int Count => Children.Count;

    public IAbstractSyntaxTreeNode this[int index] =>
        Children[index];

    public IReadOnlyList<IAbstractSyntaxTreeNode> GetAllNodes() => new TraverseEnumerator(this).ToList();


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

    public struct TraverseEnumerator : 
        IEnumerator<IAbstractSyntaxTreeNode>, 
        IEnumerable<IAbstractSyntaxTreeNode>
    {
        [ThreadStatic] private static Stack<IAbstractSyntaxTreeNode>? _buffer;

        private IAbstractSyntaxTreeNode _current;
        public IAbstractSyntaxTreeNode Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _current;
        }

        private readonly Stack<IAbstractSyntaxTreeNode> _stack;
        public TraverseEnumerator(IAbstractSyntaxTreeNode parent)
        {
            var stack = _buffer ?? new Stack<IAbstractSyntaxTreeNode>(128);
            _buffer = null;

            foreach(var child in parent)
                stack.Push(child);

            _stack = stack;
            _current = null!;
        }
        public bool MoveNext()
        {
            var stack = _stack;
            if(stack.Count == 0)
                return false;

            var current = _stack.Pop();

            foreach(var child in current)
                stack.Push(child);

            _current = current;
            return true;
        }

        public void Dispose()
        {
            _stack.Clear();
            _buffer = _stack;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TraverseEnumerator GetEnumerator() => this;
        public void Reset() { }
        IEnumerator<IAbstractSyntaxTreeNode> IEnumerable<IAbstractSyntaxTreeNode>.GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;

        object IEnumerator.Current => Current;
    }
}
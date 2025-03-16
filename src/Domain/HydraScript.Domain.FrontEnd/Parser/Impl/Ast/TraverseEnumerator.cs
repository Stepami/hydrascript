using System.Collections;
using System.Runtime.CompilerServices;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast;

/// <summary>
/// Post in Telegram: https://t.me/csharp_gepard/89
/// </summary>
internal struct TraverseEnumerator :
    IEnumerator<IAbstractSyntaxTreeNode>,
    IEnumerable<IAbstractSyntaxTreeNode>
{
    [ThreadStatic]
    private static Queue<IAbstractSyntaxTreeNode>? _buffer;

    private IAbstractSyntaxTreeNode _current;

    public IAbstractSyntaxTreeNode Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _current;
    }

    private readonly Queue<IAbstractSyntaxTreeNode> _queue;

    public TraverseEnumerator(IAbstractSyntaxTreeNode parent)
    {
        var queue = _buffer ?? new Queue<IAbstractSyntaxTreeNode>(128);
        _buffer = null;

        queue.Enqueue(parent);

        _queue = queue;
        _current = null!;
    }

    public bool MoveNext()
    {
        var queue = _queue;
        if (queue.Count == 0)
            return false;

        var current = _queue.Dequeue();

        for (int i = 0; i < current.Count; i++)
            queue.Enqueue(current[i]);

        _current = current;
        return true;
    }

    public void Dispose()
    {
        _queue.Clear();
        _buffer = _queue;
    }

    public void Reset()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<IAbstractSyntaxTreeNode> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => this;

    object IEnumerator.Current => Current;
}
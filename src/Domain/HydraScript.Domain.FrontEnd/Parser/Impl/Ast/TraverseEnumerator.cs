using System.Collections;
using System.Runtime.CompilerServices;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast
{
    /// <summary>
    /// Post in Telegram: https://t.me/csharp_gepard/89
    /// </summary>
    internal struct TraverseEnumerator :
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

            for (int i = 0; i < parent.Count; i++)
                stack.Push(parent[i]);

            _stack = stack;
            _current = null!;
        }
        public bool MoveNext()
        {
            var stack = _stack;
            if (stack.Count == 0)
                return false;

            var current = _stack.Pop();

            for(int i=0; i< current.Count; i++)
                stack.Push(current[i]);

            _current = current;
            return true;
        }

        public void Dispose()
        {
            _stack.Clear();
            _buffer = _stack;
        }

        public void Reset() { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<IAbstractSyntaxTreeNode> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;

        object IEnumerator.Current => Current;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast
{
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

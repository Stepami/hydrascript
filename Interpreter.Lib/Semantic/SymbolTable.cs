using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.Semantic.Symbols;

namespace Interpreter.Lib.Semantic
{
    public class SymbolTable : IDisposable
    {
        private readonly Dictionary<string, Symbol> _symbols = new();

        private SymbolTable _openScope;

        private List<SymbolTable> _subScopes = new();

        public void AddOpenScope(SymbolTable table)
        {
            _openScope = table;
            table._subScopes.Add(this);
        }

        public void AddSymbol(Symbol symbol) => _symbols[symbol.Id] = symbol;

        /// <summary>
        /// Поиск эффективного символа
        /// </summary>
        public T FindSymbol<T>(string id) where T : Symbol
        {
            var hasInsideTheScope = _symbols.TryGetValue(id, out var symbol);
            return !hasInsideTheScope ? _openScope?.FindSymbol<T>(id) : symbol as T;
        }

        /// <summary>
        /// Проверяет наличие собственного символа
        /// </summary>
        public bool ContainsSymbol(string id) => _symbols.ContainsKey(id);

        public void Clear() => _symbols.Clear();

        private void DeepClean()
        {
            Clear();
            foreach (var child in _subScopes)
            {
                child.DeepClean();
            }
        }

        [SuppressMessage("ReSharper", "CA1816")]
        public void Dispose()
        {
            var table = _openScope;
            while (table._openScope != null)
            {
                table = table._openScope;
            }

            table.DeepClean();
        }
    }
}
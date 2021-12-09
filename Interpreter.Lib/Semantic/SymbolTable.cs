using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.Semantic.Symbols;

namespace Interpreter.Lib.Semantic
{
    public class SymbolTable : IDisposable
    {
        private readonly Dictionary<string, Symbol> symbols = new();

        private SymbolTable openScope;

        private List<SymbolTable> subScopes = new();

        public void AddOpenScope(SymbolTable table)
        {
            openScope = table;
            table.subScopes.Add(this);
        }

        public void AddSymbol(Symbol symbol) => symbols[symbol.Id] = symbol;

        /// <summary>
        /// Поиск эффективного символа
        /// </summary>
        public T FindSymbol<T>(string id) where T : Symbol
        {
            var hasInsideTheScope = symbols.TryGetValue(id, out var symbol);
            return !hasInsideTheScope ? openScope?.FindSymbol<T>(id) : symbol as T;
        }

        /// <summary>
        /// Проверяет наличие собственного символа
        /// </summary>
        public bool ContainsSymbol(string id) => symbols.ContainsKey(id);

        public void Clear() => symbols.Clear();

        private void DeepClean()
        {
            Clear();
            foreach (var child in subScopes)
            {
                child.DeepClean();
            }
        }

        [SuppressMessage("ReSharper", "CA1816")]
        public void Dispose()
        {
            var table = openScope;
            while (table.openScope != null)
            {
                table = table.openScope;
            }

            table.DeepClean();
        }
    }
}
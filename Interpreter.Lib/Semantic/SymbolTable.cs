using System.Collections.Generic;
using Interpreter.Lib.Semantic.Symbols;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic
{
    public class SymbolTable
    {
        private readonly Dictionary<string, Symbol> _symbols = new();
        private readonly Dictionary<string, Type> _types = new();
        
        private SymbolTable _openScope;

        public void AddOpenScope(SymbolTable table)
        {
            _openScope = table;
        }

        public void AddSymbol(Symbol symbol) => _symbols[symbol.Id] = symbol;

        public void AddType(Type type, string typeId = null) =>
            _types[typeId ?? type.ToString()] = type;
        
        public Type FindType(string typeId)
        {
            var hasInsideTheScope = _types.TryGetValue(typeId, out var type);
            return !hasInsideTheScope ? _openScope?.FindType(typeId) : type;
        }

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
    }
}
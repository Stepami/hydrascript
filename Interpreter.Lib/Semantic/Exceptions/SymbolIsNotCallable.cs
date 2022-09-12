using Interpreter.Lib.FrontEnd.GetTokens;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class SymbolIsNotCallable: SemanticException
    {
        public SymbolIsNotCallable(string symbol, Segment segment) : 
            base($"{segment} Symbol is not callable: {symbol}")
        {
        }
    }
}
using System.Collections;
using HydraScript.Domain.FrontEnd.Lexer;

namespace HydraScript.Domain.FrontEnd.Parser.Impl;

public class TokensStream : IEnumerator<Token>
{
    private readonly IEnumerator<Token> _inner;

    private TokensStream(IEnumerator<Token> enumerator)
    {
        _inner = enumerator;
        _inner.MoveNext();
    }

    public bool MoveNext() => _inner.MoveNext();

    public void Reset() => _inner.Reset();

    public Token Current => _inner.Current;

    object IEnumerator.Current => Current;

    public void Dispose() => _inner.Dispose();

    public static implicit operator TokensStream(List<Token> tokens) => 
        new (tokens.GetEnumerator());
}
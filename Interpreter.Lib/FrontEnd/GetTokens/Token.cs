using System;
using System.Text.RegularExpressions;
using Interpreter.Lib.FrontEnd.GetTokens.TokenTypes;

namespace Interpreter.Lib.FrontEnd.GetTokens
{
    public class Token : ICloneable, IEquatable<Token>
    {
        public Segment Segment { get; }

        public Token(TokenType type)
        {
            Type = type;
        }

        public Token(TokenType type, Segment segment, string value) : this(type)
        {
            (Segment, Value) = (segment, value);
        }

        public TokenType Type { get; }

        public string Value { get; }

        public override string ToString()
        {
            var displayValue = Value;
            if (displayValue != null) displayValue = Regex.Replace(Value, "\"", "\\\"");
            if (Type.WhiteSpace()) displayValue = "";
            return $"{Type} {Segment}: {displayValue}";
        }

        public object Clone()
        {
            return new Token(Type, Segment, Value);
        }

        public override bool Equals(object obj) => Equals(obj as Token);

        public override int GetHashCode() => HashCode.Combine(Type, Segment, Value);

        public bool Equals(Token obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return Type == obj.Type && Equals(Segment, obj.Segment) && Value == obj.Value;
        }
    }
}
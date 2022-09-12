using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interpreter.Lib.FrontEnd.GetTokens
{
    public class Coordinates : IEquatable<Coordinates>
    {
        [JsonProperty] private readonly int _line, _column;

        public Coordinates(int line, int column)
        {
            (_line, _column) = (line, column);
        }

        public Coordinates(int absolutePos, List<int> system)
        {
            for (var i = 0; i < system.Count; i++)
                if (absolutePos <= system[i])
                {
                    var offset = i == 0 ? -1 : system[i - 1];
                    _line = i + 1;
                    _column = absolutePos - offset;
                    break;
                }

            if (_line == 0)
            {
                _column = 1;
                _line = system.Count + 1;
            }
        }

        public override string ToString()
        {
            return $"({_line}, {_column})";
        }
        
        public override bool Equals(object obj) => Equals(obj as Coordinates);

        public override int GetHashCode() => HashCode.Combine(_line, _column);

        public bool Equals(Coordinates obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return _line == obj._line && _column == obj._column;
        }
    }
}
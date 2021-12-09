using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interpreter.Lib.RBNF.Analysis.Lexical
{
    public class Coordinates : IEquatable<Coordinates>
    {
        [JsonProperty] private readonly int line, column;

        public Coordinates(int line, int column)
        {
            (this.line, this.column) = (line, column);
        }

        public Coordinates(int absolutePos, List<int> system)
        {
            for (var i = 0; i < system.Count; i++)
                if (absolutePos <= system[i])
                {
                    var offset = i == 0 ? -1 : system[i - 1];
                    line = i + 1;
                    column = absolutePos - offset;
                    break;
                }

            if (line == 0)
            {
                column = 1;
                line = system.Count + 1;
            }
        }

        public override string ToString()
        {
            return $"({line}, {column})";
        }
        
        public override bool Equals(object obj) => Equals(obj as Coordinates);

        public override int GetHashCode() => HashCode.Combine(line, column);

        public bool Equals(Coordinates obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return line == obj.line && column == obj.column;
        }
    }
}
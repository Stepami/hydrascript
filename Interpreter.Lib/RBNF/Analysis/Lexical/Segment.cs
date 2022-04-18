using System;
using Newtonsoft.Json;

namespace Interpreter.Lib.RBNF.Analysis.Lexical
{
    public class Segment : IEquatable<Segment>
    {
        [JsonProperty] private readonly Coordinates _start, _end;

        public Segment(Coordinates start, Coordinates end)
        {
            (_start, _end) = (start, end);
        }

        public override string ToString()
        {
            return $"{_start}-{_end}";
        }
        
        public override bool Equals(object obj) => Equals(obj as Segment);

        public override int GetHashCode() => HashCode.Combine(_start, _end);

        public bool Equals(Segment obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return Equals(_start, obj._start) && Equals(_end, obj._end);
        }

        public static Segment operator +(Segment left, Segment right) => 
            new(left._start, right._end);
    }
}
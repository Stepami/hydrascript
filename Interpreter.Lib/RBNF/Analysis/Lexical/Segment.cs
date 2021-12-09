using System;
using Newtonsoft.Json;

namespace Interpreter.Lib.RBNF.Analysis.Lexical
{
    public class Segment : IEquatable<Segment>
    {
        [JsonProperty] private readonly Coordinates start, end;

        public Segment(Coordinates start, Coordinates end)
        {
            (this.start, this.end) = (start, end);
        }

        public override string ToString()
        {
            return $"{start}-{end}";
        }
        
        public override bool Equals(object obj) => Equals(obj as Segment);

        public override int GetHashCode() => HashCode.Combine(start, end);

        public bool Equals(Segment obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return Equals(start, obj.start) && Equals(end, obj.end);
        }
    }
}
using System.Diagnostics.CodeAnalysis;

namespace Interpreter.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public record TokenTypeModel
    {
        public string Tag { get; init; }
        public string Pattern { get; init; }
        public int Priority { get; init; }
        public bool WhiteSpace { get; init; }
    }
}
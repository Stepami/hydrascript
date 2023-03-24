using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class OutsideOfStatement : SemanticException
{
    public OutsideOfStatement(Segment segment, string keyword, string statement) :
        base(segment, $"Jump \"{keyword}\" outside of statement \"{statement}\"") { }
}
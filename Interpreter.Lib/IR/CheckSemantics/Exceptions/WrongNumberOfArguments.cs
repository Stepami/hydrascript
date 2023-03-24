using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongNumberOfArguments : SemanticException
{
    public WrongNumberOfArguments(Segment segment, int expected, int actual) :
        base(segment, $"Wrong number of arguments: expected {expected}, actual {actual}") { }
}
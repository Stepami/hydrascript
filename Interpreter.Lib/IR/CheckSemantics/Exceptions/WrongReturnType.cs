using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongReturnType : SemanticException
{
    public WrongReturnType(Segment segment, Type expected, Type actual) :
        base(segment, $"Wrong return type: expected {expected}, actual {actual}") { }
}
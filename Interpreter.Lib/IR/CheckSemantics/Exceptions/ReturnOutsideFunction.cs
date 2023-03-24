using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ReturnOutsideFunction : SemanticException
{
    public ReturnOutsideFunction(Segment segment) :
        base(segment, "\"return\" outside function") { }
}
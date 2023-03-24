using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class CannotDefineType : SemanticException
{
    public CannotDefineType(Segment segment) :
        base(segment, "Cannot define type") { }
}
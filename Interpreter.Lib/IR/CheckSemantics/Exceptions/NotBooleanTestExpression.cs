using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class NotBooleanTestExpression : SemanticException
{
    public NotBooleanTestExpression(Segment segment, Type type) :
        base(segment, $"Type of expression is {type} but expected boolean") { }
}
using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongArrayLiteralDeclaration : SemanticException
{
    public WrongArrayLiteralDeclaration(Segment segment, Type type) : 
        base(segment, $"{segment} Wrong array literal declaration: all array elements must be of type {type}") { }
}
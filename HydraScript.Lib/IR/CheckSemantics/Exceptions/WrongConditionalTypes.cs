using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongConditionalTypes : SemanticException
{
    public WrongConditionalTypes(Segment cSegment, Type cType, Segment aSegment, Type aType) :
        base(cSegment + aSegment, $"Different types in conditional:  {cSegment} consequent - {cType}, {aSegment} alternate {aType}") { }
}
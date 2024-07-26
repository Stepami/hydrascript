using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongConditionalTypes(
    string segment,
    string cSegment,
    Type cType,
    string aSegment,
    Type aType) : SemanticException(
        segment,
        $"Different types in conditional:  {cSegment} consequent - {cType}, {aSegment} alternate {aType}");
using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongArrayLiteralDeclaration(string segment, Type type) :
    SemanticException(
        segment, 
        $"Wrong array literal declaration: all array elements must be of type {type}");
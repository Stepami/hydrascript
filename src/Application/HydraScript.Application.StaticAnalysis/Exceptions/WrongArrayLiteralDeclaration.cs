using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongArrayLiteralDeclaration(string segment, Type type) :
    SemanticException(
        segment, 
        $"Wrong array literal declaration: all array elements must be of type {type}");
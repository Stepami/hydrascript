using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class CannotAssignNullWhenUndefined(string segment) :
    SemanticException(segment, "Cannot assign 'null' when type is undefined");
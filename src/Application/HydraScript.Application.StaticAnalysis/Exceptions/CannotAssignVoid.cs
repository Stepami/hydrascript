using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class CannotAssignVoid(string segment) : SemanticException(segment, "Cannot assign void");
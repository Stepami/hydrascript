using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class CannotDefineType(string segment) : SemanticException(segment, "Cannot define type");
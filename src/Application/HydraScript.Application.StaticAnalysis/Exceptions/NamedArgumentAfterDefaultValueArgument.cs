using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class NamedArgumentAfterDefaultValueArgument(string segment, string function, IFunctionArgument argument) :
    SemanticException(segment, $"The argument {argument} of function {function} is placed after default value argument");
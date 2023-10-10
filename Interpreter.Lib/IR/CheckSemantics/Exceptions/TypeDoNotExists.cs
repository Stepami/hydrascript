using System.Diagnostics.CodeAnalysis;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class TypeDoNotExists : SemanticException
{
    public TypeDoNotExists(string typeId) :
        base($"Type '{typeId}' do not exists") { }
}
namespace Interpreter.Lib.IR.CheckSemantics.Visitors.SemanticChecker.Service;

public interface IDefaultValueForTypeCalculator
{
    public object GetDefaultValueForType(Type type);
}
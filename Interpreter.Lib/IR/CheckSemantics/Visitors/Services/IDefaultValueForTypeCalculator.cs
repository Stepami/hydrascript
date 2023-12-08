namespace Interpreter.Lib.IR.CheckSemantics.Visitors.Services;

public interface IDefaultValueForTypeCalculator
{
    public object GetDefaultValueForType(Type type);
}
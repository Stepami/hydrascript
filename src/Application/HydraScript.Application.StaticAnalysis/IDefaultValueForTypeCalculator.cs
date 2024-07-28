namespace HydraScript.Application.StaticAnalysis;

public interface IDefaultValueForTypeCalculator
{
    public object? GetDefaultValueForType(Type type);
}
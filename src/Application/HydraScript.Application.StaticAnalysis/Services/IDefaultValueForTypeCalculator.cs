namespace HydraScript.Application.StaticAnalysis.Services;

public interface IDefaultValueForTypeCalculator
{
    public object? GetDefaultValueForType(Type type);
}
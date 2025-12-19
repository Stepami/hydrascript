namespace HydraScript.Application.StaticAnalysis;

public interface IExplicitCastValidator
{
    bool IsAllowed(Type from, Type to);
}
namespace HydraScript.Application.StaticAnalysis;

public interface IComputedTypesStorage
{
    public Guid Save(Type computedType);

    public Type Get(Guid computedTypeGuid);
}
namespace HydraScript.Application.StaticAnalysis.Services;

public interface IComputedTypesStorage
{
    public Guid Save(Type computedType);

    public Type Get(Guid computedTypeGuid);
}
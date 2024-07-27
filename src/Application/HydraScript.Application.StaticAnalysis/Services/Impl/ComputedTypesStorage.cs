namespace HydraScript.Application.StaticAnalysis.Services.Impl;

public class ComputedTypesStorage : IComputedTypesStorage
{
    private readonly Dictionary<Guid, Type> _computedTypes = [];

    public Guid Save(Type computedType)
    {
        var guid = Guid.NewGuid();
        _computedTypes[guid] = computedType;
        return guid;
    }

    public Type Get(Guid computedTypeGuid) =>
        _computedTypes[computedTypeGuid];
}
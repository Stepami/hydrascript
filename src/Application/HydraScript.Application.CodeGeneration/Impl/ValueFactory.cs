using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Frames;
using HydraScript.Domain.BackEnd.Impl.Values;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.CodeGeneration.Impl;

internal class ValueFactory(
    IFrameContext frameContext,
    IEnvironmentVariableProvider provider) : IValueFactory
{
    public IValue Create(ValueDto dto) =>
        dto switch
        {
            { Type: ValueDtoType.Constant, Label: not null } =>
                new Constant(dto.Value, dto.Label),
            { Type: ValueDtoType.Name, Name: not null } =>
                CreateName(dto.Name),
            { Type: ValueDtoType.Env, Name: not null } =>
                CreateName(dto.Name, env: true),
            _ => throw new ArgumentOutOfRangeException(nameof(dto))
        };

    public Name CreateName(string id, bool env = false) =>
        env ? new Name(id, EnvFrame) : new Name(id, CurrentFrame);

    private CurrentFrame CurrentFrame { get; } = new(frameContext);

    private EnvFrame EnvFrame { get; } = new(provider);
}
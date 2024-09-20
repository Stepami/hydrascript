using HydraScript.Application.CodeGeneration.Impl;
using HydraScript.Application.CodeGeneration.Visitors;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.FrontEnd.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace HydraScript.Application.CodeGeneration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeGeneration(this IServiceCollection services)
    {
        services.AddSingleton<IValueDtoConverter, ValueDtoConverter>();
        services.AddKeyedSingleton<
            IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions>,
            InstructionProvider>(CodeGeneratorType.General);
        services.AddKeyedSingleton<
            IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions>,
            ExpressionInstructionProvider>(CodeGeneratorType.Expression);
        return services;
    }
}
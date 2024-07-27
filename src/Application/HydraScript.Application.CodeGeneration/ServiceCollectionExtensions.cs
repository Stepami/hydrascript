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
        services.AddTransient<IValueDtoConverter, ValueDtoConverter>();
        services.AddKeyedTransient<
            IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions>,
            InstructionProvider>("instructions");
        services.AddKeyedTransient<
            IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions>,
            ExpressionInstructionProvider>("expression-instructions");
        return services;
    }
}
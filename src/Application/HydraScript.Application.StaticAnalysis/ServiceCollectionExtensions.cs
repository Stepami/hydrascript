using HydraScript.Application.StaticAnalysis.Impl;
using HydraScript.Application.StaticAnalysis.Visitors;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using Microsoft.Extensions.DependencyInjection;

namespace HydraScript.Application.StaticAnalysis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStaticAnalysis(this IServiceCollection services)
    {
        services.AddSingleton<IFunctionWithUndefinedReturnStorage, FunctionWithUndefinedReturnStorage>();
        services.AddSingleton<IMethodStorage, MethodStorage>();
        services.AddSingleton<ISymbolTableStorage, SymbolTableStorage>();

        services.AddScoped<IComputedTypesStorage, ComputedTypesStorage>();
        services.AddScoped<ITypeDeclarationsResolver, TypeDeclarationsResolver>();

        services.AddTransient<IStandardLibraryProvider, StandardLibraryProvider>();
        services.AddTransient<IJavaScriptTypesProvider, JavaScriptTypesProvider>();
        services.AddTransient<IDefaultValueForTypeCalculator, DefaultValueForTypeCalculator>();

        services.AddTransient<IVisitor<TypeValue, Type>, TypeBuilder>();

        services.AddTransient<IVisitor<IAbstractSyntaxTreeNode>, SymbolTableInitializer>();
        services.AddTransient<IVisitor<IAbstractSyntaxTreeNode>, TypeSystemLoader>();
        services.AddTransient<IVisitor<IAbstractSyntaxTreeNode>, DeclarationVisitor>();

        services.AddTransient<IVisitor<IAbstractSyntaxTreeNode, Type>, SemanticChecker>();

        return services;
    }
}
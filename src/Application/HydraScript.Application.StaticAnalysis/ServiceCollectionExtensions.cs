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

        services.AddSingleton<IComputedTypesStorage, ComputedTypesStorage>();
        services.AddSingleton<ITypeDeclarationsResolver, TypeDeclarationsResolver>();

        services.AddSingleton<IStandardLibraryProvider, StandardLibraryProvider>();
        services.AddSingleton<IJavaScriptTypesProvider, JavaScriptTypesProvider>();
        services.AddSingleton<IDefaultValueForTypeCalculator, DefaultValueForTypeCalculator>();

        services.AddSingleton<IVisitor<TypeValue, Type>, TypeBuilder>();

        services.AddSingleton<IVisitor<IAbstractSyntaxTreeNode>, SymbolTableInitializer>();
        services.AddSingleton<IVisitor<IAbstractSyntaxTreeNode>, TypeSystemLoader>();
        services.AddSingleton<IVisitor<IAbstractSyntaxTreeNode>, DeclarationVisitor>();

        services.AddSingleton<IVisitor<IAbstractSyntaxTreeNode, Type>, SemanticChecker>();

        return services;
    }
}
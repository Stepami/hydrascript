using System.IO.Abstractions;
using HydraScript.Application.CodeGeneration;
using HydraScript.Application.StaticAnalysis;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl;
using HydraScript.Infrastructure.Dumping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HydraScript.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain<TGeneratedRegexContainer>(this IServiceCollection services)
        where TGeneratedRegexContainer : class, IGeneratedRegexContainer
    {
        services.AddSingleton<ITextCoordinateSystemComputer, TextCoordinateSystemComputer>();
        services.AddSingleton<ITokenTypesProvider, TokenTypesProvider>();
        services.AddSingleton<IStructure, Structure<TGeneratedRegexContainer>>();
        services.AddSingleton<ILexer, RegexLexer>();
        services.AddSingleton<IParser, TopDownParser>();

        services.AddSingleton<IFrameContext, FrameContext>();
        services.AddSingleton(SystemEnvironmentVariableProvider.Instance);
        services.AddSingleton<IOutputWriter, LoggingWriter>();
        services.AddSingleton<IVirtualMachine, VirtualMachine>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services) => services
        .AddStaticAnalysis()
        .AddCodeGeneration();

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        bool dump,
        FileInfo fileInfo)
    {
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton(Options.Create(fileInfo));

        services.AddSingleton<IStaticAnalyzer, StaticAnalyzer>();
        services.AddSingleton<ICodeGenerator, CodeGenerator>();

        services.AddSingleton<ISourceCodeProvider, SourceCodeProvider>();
        services.AddSingleton<IDumpingService, DumpingService>();

        if (dump)
        {
            services.AddKeyedSingleton<ILexer, RegexLexer>(DecoratorKey.Value);
            services.AddSingleton<ILexer, DumpingLexer>();

            services.AddKeyedSingleton<IParser, TopDownParser>(DecoratorKey.Value);
            services.AddSingleton<IParser, DumpingParser>();

            services.AddKeyedSingleton<IVirtualMachine, VirtualMachine>(DecoratorKey.Value);
            services.AddSingleton<IVirtualMachine, DumpingVirtualMachine>();
        }

        services.AddSingleton<Executor>();

        return services;
    }
}
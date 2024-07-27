using System.IO.Abstractions;
using HydraScript.Application.CodeGeneration;
using HydraScript.Application.StaticAnalysis;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl;
using HydraScript.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HydraScript.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddSingleton<ITextCoordinateSystemComputer, TextCoordinateSystemComputer>();
        services.AddSingleton(StructureInstance.Get);
        services.AddSingleton<ILexer, RegexLexer>();
        services.AddSingleton<IParser, TopDownParser>();

        services.AddSingleton(Console.Out);
        services.AddSingleton<IVirtualMachine, VirtualMachine>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services) => services
        .AddStaticAnalysis()
        .AddCodeGeneration();

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        bool dump,
        string inputFilePath)
    {
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton(Options.Create(new InputFile { Path = inputFilePath }));

        services.AddTransient<IStaticAnalyzer, StaticAnalyzer>();
        services.AddTransient<ICodeGenerator, CodeGenerator>();

        services.AddTransient<ISourceCodeProvider, SourceCodeProvider>();

        if (dump)
        {
            services.Decorate<ILexer, LoggingLexer>();
            services.Decorate<IParser, LoggingParser>();
            services.Decorate<IVirtualMachine, LoggingVirtualMachine>();
        }

        return services;
    }
} 
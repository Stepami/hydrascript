using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using CommandLine;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.GetTokens.Impl;
using HydraScript.Services.CodeGen;
using HydraScript.Services.CodeGen.Impl;
using HydraScript.Services.Executor;
using HydraScript.Services.Executor.Impl;
using HydraScript.Services.Parsing;
using HydraScript.Services.Parsing.Impl;
using HydraScript.Services.Providers.LexerProvider;
using HydraScript.Services.Providers.LexerProvider.Impl;
using HydraScript.Services.Providers.ParserProvider;
using HydraScript.Services.Providers.ParserProvider.Impl;
using HydraScript.Services.Providers.StructureProvider;
using HydraScript.Services.Providers.StructureProvider.Impl;
using HydraScript.Services.SourceCode;
using HydraScript.Services.SourceCode.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HydraScript;

[ExcludeFromCodeCoverage]
public static class Program
{
    private static IServiceCollection ServiceCollection { get; } = new ServiceCollection();
    private static IServiceProvider? ServiceProvider { get; set; }

    private static void Main(string[] args) =>
        Parser.Default.ParseArguments<CommandLineSettings>(args)
            .WithParsed(options =>
            {
                ConfigureServices(options);
                ServiceProvider?
                    .GetService<IExecutor>()!
                    .Execute();
            })
            .WithNotParsed(errors => errors.Output());
        

    private static void ConfigureServices(CommandLineSettings settings)
    {
        ServiceCollection.AddSingleton<IStructureProvider, StructureProvider>();
        ServiceCollection.AddSingleton<ILexerProvider, LexerProvider>();
        ServiceCollection.AddSingleton<IParserProvider, ParserProvider>();
        ServiceCollection.AddSingleton<IParsingService, ParsingService>();
        ServiceCollection.AddSingleton<ISourceCodeProvider, SourceCodeProvider>();
        ServiceCollection.AddSingleton<IFileSystem, FileSystem>();
        ServiceCollection.AddSingleton<ITextCoordinateSystemComputer, TextCoordinateSystemComputer>();
        ServiceCollection.AddSingleton<ICodeGenService, CodeGenService>();

        ServiceCollection.AddSingleton<IExecutor, Executor>();

        ServiceCollection.AddSingleton(_ => Options.Create(settings));
            
        ServiceProvider = ServiceCollection.BuildServiceProvider();
    }
}
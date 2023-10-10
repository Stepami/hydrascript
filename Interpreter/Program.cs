using System.Diagnostics.CodeAnalysis;
using CommandLine;
using Interpreter.Services.Executor;
using Interpreter.Services.Executor.Impl;
using Interpreter.Services.Parsing;
using Interpreter.Services.Parsing.Impl;
using Interpreter.Services.Providers.LexerProvider;
using Interpreter.Services.Providers.LexerProvider.Impl;
using Interpreter.Services.Providers.ParserProvider;
using Interpreter.Services.Providers.ParserProvider.Impl;
using Interpreter.Services.Providers.StructureProvider;
using Interpreter.Services.Providers.StructureProvider.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Interpreter;

[ExcludeFromCodeCoverage]
public static class Program
{
    private static IServiceCollection ServiceCollection { get; } = new ServiceCollection();
    private static IServiceProvider ServiceProvider { get; set; }

    private static void Main(string[] args) =>
        Parser.Default.ParseArguments<CommandLineSettings>(args)
            .WithParsed(options =>
            {
                ConfigureServices(options);
                ServiceProvider
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

        ServiceCollection.AddSingleton<IExecutor, Executor>();

        ServiceCollection.AddSingleton(_ => Options.Create(settings));
            
        ServiceProvider = ServiceCollection.BuildServiceProvider();
    }
}
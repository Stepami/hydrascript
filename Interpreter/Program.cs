using System;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Interpreter.MappingProfiles;
using Interpreter.Services.Executor;
using Interpreter.Services.Executor.Impl;
using Interpreter.Services.Parsing;
using Interpreter.Services.Parsing.Impl;
using Interpreter.Services.Providers;
using Interpreter.Services.Providers.Impl;
using Microsoft.Extensions.Options;

namespace Interpreter
{
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
            ServiceCollection.AddSingleton<ILexerProvider, LexerProvider>();
            ServiceCollection.AddSingleton<IParserProvider, ParserProvider>();
            ServiceCollection.AddSingleton<IParsingService, ParsingService>();

            ServiceCollection.AddAutoMapper(typeof(TokenTypeProfile), typeof(StructureProfile));

            ServiceCollection.AddSingleton<IExecutor, Executor>();

            ServiceCollection.AddSingleton(_ => Options.Create(settings));
            
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
    }
}
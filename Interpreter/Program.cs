using System;
using System.Diagnostics.CodeAnalysis;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Interpreter.MappingProfiles;
using Interpreter.Services.Executor;
using Interpreter.Services.Executor.Impl;
using Interpreter.Services.Providers;
using Interpreter.Services.Providers.Impl;

namespace Interpreter
{
    public static class Program
    {
        private static IServiceCollection ServiceCollection { get; } = new ServiceCollection();
        private static IServiceProvider ServiceProvider { get; set; }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private static void Main(string[] args)
        {
            ConfigureServices();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => ServiceProvider
                    .GetService<IExecutor>()
                    .Execute(options))
                .WithNotParsed(errors => errors.Output());
        }

        private static void ConfigureServices()
        {
            ServiceCollection.AddTransient<ILexerProvider, LexerProvider>();
            ServiceCollection.AddTransient<IParserProvider, ParserProvider>();

            ServiceCollection.AddAutoMapper(typeof(TokenTypeProfile));

            ServiceCollection.AddSingleton<IExecutor, Executor>();
            
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
    }
}
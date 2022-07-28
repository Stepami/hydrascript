using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Interpreter.MappingProfiles;
using Interpreter.Models;
using Interpreter.Services.Executor;
using Interpreter.Services.Executor.Impl;
using Interpreter.Services.Providers;
using Interpreter.Services.Providers.Impl;

namespace Interpreter
{
    public static class Program
    {
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
        public class Options
        {
            [Value(0, MetaName = "InputFilePath", Required = true, HelpText = "Path to input file")]
            public string InputFilePath { get; set; }

            [Option('d', "dump", Default = false, HelpText = "Show dump data of interpreter")]
            public bool Dump { get; set; }

            [Usage(ApplicationAlias = "Interpreter")]
            public static IEnumerable<Example> Examples
            {
                get
                {
                    yield return new Example("Simple interpretation call", 
                        new Options { InputFilePath = "file.js" });
                    yield return new Example("Request dump",
                        new Options { InputFilePath = "file.js", Dump = true });
                }
            }

            public string GetInputFileName() => InputFilePath.Split(' ')[0];

            public LexerQueryModel CreateLexerQuery() =>
                new()
                {
                    Text = File.ReadAllText(InputFilePath)
                };
        }

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
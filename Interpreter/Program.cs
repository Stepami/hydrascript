using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CliWrap;
using CommandLine;
using CommandLine.Text;
using Interpreter.Lib.IR;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.IR.Optimizers;
using Interpreter.Lib.RBNF.Analysis.Exceptions;
using Interpreter.Lib.Semantic.Analysis;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.VM;
using Microsoft.Extensions.DependencyInjection;
using Interpreter.Services;
using Interpreter.MappingProfiles;

namespace Interpreter
{
    public static class Program
    {
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        // ReSharper disable once ClassNeverInstantiated.Global
        public class Options
        {
            [Option('t', Default = "tokenTypes.json", HelpText = "Path to lexer configuration")]
            public string TokenTypesJsonFilePath { get; set; }

            [Option('d', Default = false, HelpText = "Show dump data of interpreter")]
            public bool Dump { get; set; }

            [Option('i', Required = true, HelpText = "Path to input file")]
            public string InputFilePath { get; set; }
        }

        private static IServiceCollection ServiceCollection { get; } = new ServiceCollection();
        private static IServiceProvider ServiceProvider { get; set; }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private static async Task Main(string[] args)
        {
            ConfigureServices();

            var parserResult = Parser.Default.ParseArguments<Options>(args);

            await parserResult.WithParsedAsync(async o =>
            {
                try
                {
                    var creator = new QueryCreator(o);

                    var lexer = ServiceProvider.GetService<ILexerCreatorService>()
                        .CreateLexer(creator.CreateLexerQuery());

                    var parser = ServiceProvider.GetService<IParserCreatorService>()
                        .CreateParser(lexer);

                    using var ast = parser.TopDownParse();

                    ast.Check(new SemanticAnalyzer(node => node.SemanticCheck()));

                    var instructions = ast.GetInstructions();

                    var cfg = new ControlFlowGraph(
                        new BasicBlockBuilder(instructions)
                            .GetBasicBlocks()
                    );

                    cfg.OptimizeInstructions(i => new IdentityExpression(i as Simple));

                    if (o.Dump)
                    {
                        var fileName = o.InputFilePath.Split(".js")[0];
                        await File.WriteAllLinesAsync(
                            $"{fileName}.tac",
                            instructions.Select(i => i.ToString())
                        );

                        await File.WriteAllTextAsync(
                            $"{fileName}.tokens",
                            string.Join('\n', lexer)
                        );

                        var astDot = ast.ToString();
                        await File.WriteAllTextAsync("ast.dot", astDot);
                        await Cli.Wrap("dot")
                            .WithArguments("-Tpng ast.dot -o ast.png")
                            .ExecuteAsync();

                        var cfgDot = cfg.ToString();
                        await File.WriteAllTextAsync("cfg.dot", cfgDot);
                        await Cli.Wrap("dot")
                            .WithArguments("-Tpng cfg.dot -o cfg.png")
                            .ExecuteAsync();
                    }

                    var vm = new VirtualMachine(cfg);
                    vm.Run();
                }
                catch (Exception ex)
                    when (ex is LexerException || ex is ParserException || ex is SemanticException)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Internal Interpreter Error");
                    Console.WriteLine(ex);
                }
            });
            await parserResult.WithNotParsedAsync(_ =>
            {
                HelpText.AutoBuild(parserResult);
                return Task.CompletedTask;
            });
        }

        private static void ConfigureServices()
        {
            ServiceCollection.AddTransient<ILexerCreatorService, LexerCreatorService>();
            ServiceCollection.AddTransient<IParserCreatorService, ParserCreatorService>();

            ServiceCollection.AddAutoMapper(typeof(TokenTypeProfile));

            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
    }
}
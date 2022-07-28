using System;
using System.IO;
using System.Linq;
using Interpreter.Lib.IR;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.IR.Optimizers;
using Interpreter.Lib.RBNF.Analysis.Exceptions;
using Interpreter.Lib.Semantic.Analysis;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.VM;
using Interpreter.Services.Providers;

namespace Interpreter.Services.Executor.Impl
{
    public class Executor : IExecutor
    {
        private readonly ILexerProvider _lexerProvider;
        private readonly IParserProvider _parserProvider;

        public Executor(ILexerProvider lexerProvider, IParserProvider parserProvider)
        {
            _lexerProvider = lexerProvider;
            _parserProvider = parserProvider;
        }

        public void Execute(Options options)
        {
            try
            {
                var lexer = _lexerProvider
                    .CreateLexer(options.CreateLexerQuery());

                var parser = _parserProvider
                    .CreateParser(lexer);

                using var ast = parser.TopDownParse();

                ast.Check(new SemanticAnalyzer(node => node.SemanticCheck()));

                var instructions = ast.GetInstructions();

                var cfg = new ControlFlowGraph(
                    new BasicBlockBuilder(instructions)
                        .GetBasicBlocks()
                );

                cfg.OptimizeInstructions(
                    i => new IdentityExpression(i as Simple),
                    i => new ZeroExpression(i as Simple)
                );

                if (options.Dump)
                {
                    var fileName = options.GetInputFileName();
                    File.WriteAllLines(
                        $"{fileName}.tac",
                        instructions.Select(i => i.ToString())
                    );

                    File.WriteAllText(
                        $"{fileName}.tokens",
                        string.Join('\n', lexer)
                    );

                    var astDot = ast.ToString();
                    File.WriteAllText("ast.dot", astDot);

                    var cfgDot = cfg.ToString();
                    File.WriteAllText("cfg.dot", cfgDot);
                }

                var vm = new VirtualMachine(cfg);
                vm.Run();
            }
            catch (Exception ex)
                when (ex is LexerException or ParserException or SemanticException)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Internal Interpreter Error");
                Console.WriteLine(ex);
            }
        }
    }
}

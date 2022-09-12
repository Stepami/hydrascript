using System;
using System.IO;
using System.Linq;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.TopDownParse;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.VM;
using Interpreter.Services.Providers;
using Microsoft.Extensions.Options;

namespace Interpreter.Services.Executor.Impl
{
    public class Executor : IExecutor
    {
        private readonly ILexerProvider _lexerProvider;
        private readonly IParserProvider _parserProvider;
        private readonly CommandLineSettings _commandLineSettings;

        public Executor(ILexerProvider lexerProvider, IParserProvider parserProvider, IOptions<CommandLineSettings> optionsProvider)
        {
            _lexerProvider = lexerProvider;
            _parserProvider = parserProvider;
            _commandLineSettings = optionsProvider.Value;
        }

        public void Execute()
        {
            try
            {
                var lexer = _lexerProvider.CreateLexer();
                var parser = _parserProvider.CreateParser(lexer);

                var ast = parser.TopDownParse(_commandLineSettings.GetText());

                var instructions = ast.GetInstructions();

                var vm = new VirtualMachine();
                vm.Run(instructions);
                
                if (_commandLineSettings.Dump)
                {
                    var fileName = _commandLineSettings.GetInputFileName();
                    File.WriteAllLines(
                        $"{fileName}.tac",
                        instructions.OrderBy(i => i).Select(i => i.ToString())
                    );

                    File.WriteAllText(
                        $"{fileName}.tokens",
                        string.Join('\n', lexer)
                    );

                    var astDot = ast.ToString();
                    File.WriteAllText("ast.dot", astDot);
                }
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

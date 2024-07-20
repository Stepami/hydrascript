using HydraScript.Services.Parsing;
using HydraScript.Lib.BackEnd;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.TopDownParse;
using HydraScript.Lib.IR.CheckSemantics.Exceptions;
using Microsoft.Extensions.Options;

namespace HydraScript.Services.Executor.Impl;

public class Executor : IExecutor
{
    private readonly IParsingService _parsingService;
    private readonly CommandLineSettings _commandLineSettings;

    public Executor(IParsingService parsingService, IOptions<CommandLineSettings> options)
    {
        _parsingService = parsingService;
        _commandLineSettings = options.Value;
    }

    public void Execute()
    {
        try
        {
            var ast = _parsingService.Parse(_commandLineSettings.GetText());
            var instructions = ast.GetInstructions();

            var vm = new VirtualMachine();
            vm.Run(instructions);
        }
        catch (Exception ex)
            when (ex is LexerException or ParserException or SemanticException)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Internal HydraScript Error");
            Console.WriteLine(ex);
        }
    }
}
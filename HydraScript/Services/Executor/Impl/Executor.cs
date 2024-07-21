using HydraScript.Services.Parsing;
using HydraScript.Lib.BackEnd;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.TopDownParse;
using HydraScript.Lib.IR.CheckSemantics.Exceptions;
using HydraScript.Services.SourceCode;
using Microsoft.Extensions.Options;

namespace HydraScript.Services.Executor.Impl;

public class Executor : IExecutor
{
    private readonly IParsingService _parsingService;
    private readonly ISourceCodeProvider _sourceCodeProvider;

    public Executor(
        IParsingService parsingService,
        ISourceCodeProvider sourceCodeProvider)
    {
        _parsingService = parsingService;
        _sourceCodeProvider = sourceCodeProvider;
    }

    public void Execute()
    {
        try
        {
            var text = _sourceCodeProvider.GetText();
            var ast = _parsingService.Parse(text);
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
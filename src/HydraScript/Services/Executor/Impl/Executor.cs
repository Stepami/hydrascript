using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl;
using HydraScript.Services.Parsing;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.TopDownParse;
using HydraScript.Lib.IR.CheckSemantics.Exceptions;
using HydraScript.Services.CodeGen;
using HydraScript.Services.SourceCode;

namespace HydraScript.Services.Executor.Impl;

public class Executor : IExecutor
{
    private readonly IParsingService _parsingService;
    private readonly ISourceCodeProvider _sourceCodeProvider;
    private readonly ICodeGenService _codeGenService;

    public Executor(
        IParsingService parsingService,
        ISourceCodeProvider sourceCodeProvider,
        ICodeGenService codeGenService)
    {
        _parsingService = parsingService;
        _sourceCodeProvider = sourceCodeProvider;
        _codeGenService = codeGenService;
    }

    public void Execute()
    {
        try
        {
            var text = _sourceCodeProvider.GetText();
            var ast = _parsingService.Parse(text);
            var instructions = _codeGenService.GetInstructions(ast);

            IVirtualMachine vm = new VirtualMachine(Console.Out);
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
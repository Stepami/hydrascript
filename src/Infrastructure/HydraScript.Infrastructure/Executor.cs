using HydraScript.Application.CodeGeneration;
using HydraScript.Application.StaticAnalysis.Exceptions;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Parser;

namespace HydraScript.Infrastructure;

public class Executor(
    ISourceCodeProvider sourceCodeProvider,
    IParser parser,
    ICodeGenerator codeGenerator,
    IVirtualMachine virtualMachine)
{
    public int Invoke()
    {
        var writer = virtualMachine.ExecuteParams.Writer;
        try
        {
            var sourceCode = sourceCodeProvider.GetText();
            var ast = parser.Parse(sourceCode);
            var instructions = codeGenerator.GetInstructions(ast);
            virtualMachine.Run(instructions);
            return ExitCodes.Success;
        }
        catch (Exception ex)
            when (ex is LexerException or ParserException or SemanticException)
        {
            writer.WriteError(ex, message: "HydraScript Error");
            return ExitCodes.HydraScriptError;
        }
        catch (Exception ex)
        {
            writer.WriteError(ex, message: "Dotnet Runtime Error");
            return ExitCodes.DotnetRuntimeError;
        }
    }

    internal static class ExitCodes
    {
        public const int Success = 0;

        public const int HydraScriptError = 1;

        public const int DotnetRuntimeError = 2;
    }
}
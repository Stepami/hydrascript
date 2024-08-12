using System.CommandLine.Invocation;
using HydraScript.Application.CodeGeneration;
using HydraScript.Application.StaticAnalysis.Exceptions;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Infrastructure;

namespace HydraScript;

internal class ExecuteCommandHandler(
    ISourceCodeProvider sourceCodeProvider,
    IParser parser,
    ICodeGenerator codeGenerator,
    IVirtualMachine virtualMachine,
    TextWriter writer) : ICommandHandler
{

    public int Invoke(InvocationContext context)
    {
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
            writer.WriteLine(ex.Message);
            return ExitCodes.HydraScriptError;
        }
        catch (Exception ex)
        {
            writer.WriteLine("Internal HydraScript Error");
            writer.WriteLine(ex);
            return ExitCodes.DotnetRuntimeError;
        }
    }

    public Task<int> InvokeAsync(InvocationContext context) =>
        Task.FromResult(Invoke(context));
}
using HydraScript.Application.CodeGeneration;
using HydraScript.Application.StaticAnalysis;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.FrontEnd.Parser;
using Microsoft.Extensions.DependencyInjection;
using Visitor.NET;

namespace HydraScript.Infrastructure;

internal class CodeGenerator : ICodeGenerator
{
    private readonly IStaticAnalyzer _staticAnalyzer;
    private readonly IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions> _visitor;

    public CodeGenerator(
        IStaticAnalyzer staticAnalyzer,
        [FromKeyedServices(CodeGeneratorType.General)]
        IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions> visitor)
    {
        _staticAnalyzer = staticAnalyzer;
        _visitor = visitor;
    }

    public AddressedInstructions GetInstructions(IAbstractSyntaxTree ast)
    {
        _staticAnalyzer.Analyze(ast);
        var root = ast.Root;
        return root.Accept(_visitor);
    }
}
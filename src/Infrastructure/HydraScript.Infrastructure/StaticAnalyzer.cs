using HydraScript.Application.StaticAnalysis;
using HydraScript.Domain.FrontEnd.Parser;
using Visitor.NET;
using Type = HydraScript.Domain.IR.Types.Type;

namespace HydraScript.Infrastructure;

internal class StaticAnalyzer(
    IEnumerable<IVisitor<IAbstractSyntaxTreeNode>> preAnalyzers,
    IVisitor<IAbstractSyntaxTreeNode, Type> analyzer) : IStaticAnalyzer
{
    public void Analyze(IAbstractSyntaxTree ast)
    {
        var root = ast.Root;
        foreach (var preAnalyzer in preAnalyzers)
        {
            root.Accept(preAnalyzer);
        }

        root.Accept(analyzer);
    }
}
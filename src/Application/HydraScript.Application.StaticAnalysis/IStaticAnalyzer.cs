using HydraScript.Domain.FrontEnd.Parser;

namespace HydraScript.Application.StaticAnalysis;

public interface IStaticAnalyzer
{
    public void Analyze(IAbstractSyntaxTree ast);
}
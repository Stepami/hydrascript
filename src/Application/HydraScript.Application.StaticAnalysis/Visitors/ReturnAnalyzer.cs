using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.Application.StaticAnalysis.Visitors;

internal class ReturnAnalyzer : VisitorBase<IAbstractSyntaxTreeNode, ReturnAnalyzerResult>,
    IVisitor<FunctionDeclaration, ReturnAnalyzerResult>,
    IVisitor<IfStatement, ReturnAnalyzerResult>,
    IVisitor<ReturnStatement, ReturnAnalyzerResult>
{
    public ReturnAnalyzerResult Visit(FunctionDeclaration visitable)
    {
        IAbstractSyntaxTreeNode astNode = visitable;
        return Visit(astNode);
    }

    public override ReturnAnalyzerResult Visit(IAbstractSyntaxTreeNode visitable)
    {
        var result = ReturnAnalyzerResult.AdditiveIdentity;
        for (var i = 0; i < visitable.Count; i++)
        {
            var visitableResult = visitable[i].Accept(This);
            if (visitableResult.CodePathEndedWithReturn)
                return visitableResult * result;
            result += visitableResult;
        }

        return result;
    }

    public ReturnAnalyzerResult Visit(IfStatement visitable)
    {
        var thenReturns = visitable.Then.Accept(This);

        if (visitable.Else is null)
            return thenReturns + ReturnAnalyzerResult.AdditiveIdentity;
        var elseReturns = visitable.Else.Accept(This);

        return thenReturns + elseReturns;
    }

    public ReturnAnalyzerResult Visit(ReturnStatement visitable) =>
        new(CodePathEndedWithReturn: true, ReturnStatements: [visitable]);
}
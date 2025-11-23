using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.Application.StaticAnalysis.Visitors;

internal class ReturnAnalyzer : VisitorBase<IAbstractSyntaxTreeNode, bool>,
    IVisitor<FunctionDeclaration, ReturnAnalyzerResult>,
    IVisitor<IfStatement, bool>,
    IVisitor<ReturnStatement, bool>
{
    private readonly List<ReturnStatement> _returnStatements = [];

    public ReturnAnalyzerResult Visit(FunctionDeclaration visitable)
    {
        IAbstractSyntaxTreeNode astNode = visitable;
        var codePathEndedWithReturn= Visit(astNode);
        var returnStatements = new List<ReturnStatement>(_returnStatements);
        ReturnAnalyzerResult result = new(codePathEndedWithReturn, returnStatements);
        _returnStatements.Clear();
        return result;
    }

    public override bool Visit(IAbstractSyntaxTreeNode visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
        {
            var visitableResult = visitable[i].Accept(This);
            if (visitableResult)
                return true;
        }

        return false;
    }

    public bool Visit(IfStatement visitable)
    {
        var thenReturns = visitable.Then.Accept(This);

        if (visitable.Else is null)
            return false;
        var elseReturns = visitable.Else.Accept(This);

        return thenReturns && elseReturns;
    }

    public bool Visit(ReturnStatement visitable)
    {
        _returnStatements.Add(visitable);
        return true;
    }
}

public sealed record ReturnAnalyzerResult(
    bool CodePathEndedWithReturn,
    IReadOnlyList<ReturnStatement> ReturnStatements);
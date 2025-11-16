using System.Numerics;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.Application.StaticAnalysis.Visitors;

public sealed record ReturnAnalyzerResult(bool CodePathEndedWithReturn, IReadOnlyList<ReturnStatement> ReturnStatements) :
    IAdditiveIdentity<ReturnAnalyzerResult, ReturnAnalyzerResult>,
    IAdditionOperators<ReturnAnalyzerResult, ReturnAnalyzerResult, ReturnAnalyzerResult>,
    IMultiplyOperators<ReturnAnalyzerResult, ReturnAnalyzerResult, ReturnAnalyzerResult>
{
    public static ReturnAnalyzerResult operator +(ReturnAnalyzerResult left, ReturnAnalyzerResult right) =>
        new(
            left.CodePathEndedWithReturn && right.CodePathEndedWithReturn,
            ReturnStatements: [..left.ReturnStatements, ..right.ReturnStatements]);

    public static ReturnAnalyzerResult AdditiveIdentity { get; } = new(CodePathEndedWithReturn: false, ReturnStatements: []);

    public static ReturnAnalyzerResult operator *(ReturnAnalyzerResult left, ReturnAnalyzerResult right) =>
        new(
            left.CodePathEndedWithReturn || right.CodePathEndedWithReturn,
            ReturnStatements: [..left.ReturnStatements, ..right.ReturnStatements]);
}
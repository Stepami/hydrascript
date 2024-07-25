using HydraScript.Lib.IR.Ast;

namespace HydraScript.Services.Parsing;

public interface IParsingService
{
    IAbstractSyntaxTree Parse(string text);
}
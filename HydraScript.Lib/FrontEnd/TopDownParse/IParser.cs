using HydraScript.Lib.IR.Ast;

namespace HydraScript.Lib.FrontEnd.TopDownParse;

public interface IParser
{
    IAbstractSyntaxTree TopDownParse(string text);
}
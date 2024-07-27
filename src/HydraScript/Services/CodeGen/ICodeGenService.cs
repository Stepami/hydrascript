using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast;

namespace HydraScript.Services.CodeGen;

public interface ICodeGenService
{
    AddressedInstructions GetInstructions(IAbstractSyntaxTree ast);
}
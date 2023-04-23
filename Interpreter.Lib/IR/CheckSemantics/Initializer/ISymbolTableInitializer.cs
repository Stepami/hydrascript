using Interpreter.Lib.IR.Ast;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Initializer;

public interface ISymbolTableInitializer
{
    Unit InitThroughParent(AbstractSyntaxTreeNode node);

    Unit InitWithNewScope(AbstractSyntaxTreeNode node);
}
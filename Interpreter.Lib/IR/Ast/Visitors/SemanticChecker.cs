using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Visitors;

public class SemanticChecker :
    IVisitor<WhileStatement, Type>,
    IVisitor<IfStatement, Type>,
    IVisitor<InsideStatementJump, Type>,
    IVisitor<ReturnStatement, Type>
{
    public Type Visit(WhileStatement visitable)
    {
        var condType = visitable.Condition.Accept(this);
        if (!condType.Equals(TypeUtils.JavaScriptTypes.Boolean))
        {
            throw new NotBooleanTestExpression(visitable.Segment, condType);
        }

        return default;
    }
    
    public Type Visit(IfStatement visitable)
    {
        var testType = visitable.Test.Accept(this);
        if (!testType.Equals(TypeUtils.JavaScriptTypes.Boolean))
        {
            throw new NotBooleanTestExpression(visitable.Segment, testType);
        }
        
        return default;
    }
    
    public Type Visit(InsideStatementJump visitable)
    {
        switch (visitable.Keyword)
        {
            case InsideStatementJump.Break:
                if (!(visitable.ChildOf<IfStatement>() || visitable.ChildOf<WhileStatement>()))
                    throw new OutsideOfStatement(visitable.Segment, InsideStatementJump.Break, "if|while");
                break;
            case InsideStatementJump.Continue:
                if (!visitable.ChildOf<WhileStatement>())
                    throw new OutsideOfStatement(visitable.Segment, InsideStatementJump.Continue, "while");
                break;
        }

        return default;
    }

    public Type Visit(ReturnStatement visitable)
    {
        if (!visitable.ChildOf<FunctionDeclaration>())
        {
            throw new ReturnOutsideFunction(visitable.Segment);
        }

        return visitable.Expression?.NodeCheck() ?? TypeUtils.JavaScriptTypes.Void;
    }
}
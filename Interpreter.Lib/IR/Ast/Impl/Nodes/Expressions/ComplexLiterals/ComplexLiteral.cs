namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

public abstract class ComplexLiteral : Expression
{
    public string Id => Parent is AssignmentExpression assignment
        ? assignment.Destination.Id
        : $"_t{GetHashCode()}";
}
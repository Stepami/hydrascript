using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;

public class FunctionDeclaration : Declaration
{
    public string Name { get; }
    public FunctionSymbol Function { get; }
    public BlockStatement Statements { get; }

    public ObjectLiteral Object =>
        Parent as ObjectLiteral;

    public FunctionDeclaration(FunctionSymbol function, BlockStatement statements)
    {
        Function = function;
        function.Body = this;

        Name = function.Id;

        Statements = statements;
        Statements.Parent = this;
    }

    public bool HasReturnStatement() =>
        Statements.GetAllNodes()
            .OfType<ReturnStatement>()
            .Any();

    public void SetArguments(CallExpression call, List<Expression> expressions)
    {
        if (Function.Type.Arguments.Count == expressions.Count)
        {
            expressions.Select((e, i) => (e, i)).ToList()
                .ForEach(pair =>
                {
                    var (e, i) = pair;
                    var eType = e.NodeCheck();
                    if (Function.Type.Arguments[i].Equals(eType))
                    {
                        SymbolTable.AddSymbol(Function.Parameters[i]);
                    }
                    else throw new WrongTypeOfArgument(e.Segment, Function.Type.Arguments[i], eType);
                });
        }
        else throw new WrongNumberOfArguments(call.Segment, Function.Parameters.Count, expressions.Count);
    }

    public void Clear()
    {
        Statements.GetAllNodes().ForEach(x => x.SymbolTable?.Clear());
        SymbolTable.Clear();
    }

    public FunctionSymbol GetSymbol() => Function;

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Statements;
    }

    protected override string NodeRepresentation() =>
        $"function {Name}";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);

    public override Unit Accept(SymbolTableInitializer visitor) =>
        visitor.Visit(this);
}
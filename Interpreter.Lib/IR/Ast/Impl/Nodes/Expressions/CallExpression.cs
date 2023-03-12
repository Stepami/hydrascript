using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public class CallExpression : LeftHandSideExpression
{
    public MemberExpression Member { get; }
    public List<Expression> Parameters { get; }

    public CallExpression(MemberExpression member, IEnumerable<Expression> expressions)
    {
        Member = member;
        Member.Parent = this;

        Parameters = new List<Expression>(expressions);
        Parameters.ForEach(expr => expr.Parent = this);
    }
    
    public override IdentifierReference Id =>
        Member.Id;

    public override bool Empty() =>
        Member.Empty();

    private FunctionSymbol GetFunction()
    {
        if (Member.Any())
        {
            var table = SymbolTable.FindSymbol<ObjectSymbol>(Member.Id).Table;
            var chain = Member.AccessChain;
            while (chain.HasNext())
            {
                table = chain switch
                {
                    DotAccess dot => table.FindSymbol<ObjectSymbol>(dot.Property).Table,
                    IndexAccess => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
                chain = chain.Next;
            }

            return table.FindSymbol<FunctionSymbol>(((DotAccess) chain).Property);
        }

        return SymbolTable.FindSymbol<FunctionSymbol>(Member.Id);
    }

    internal override Type NodeCheck()
    {
        if (Member.Any())
        {
            Member.NodeCheck();
        }
        else
        {
            IdentifierReference idRef = Member.Id;
            idRef.NodeCheck();
        }

        var function = GetFunction();
        if (function == null)
        {
            throw new SymbolIsNotCallable(Member.Id, Segment);
        }

        if (!function.Type.ReturnType.Equals(TypeUtils.JavaScriptTypes.Void))
        {
            if (!function.Body.HasReturnStatement())
            {
                throw new FunctionWithoutReturnStatement(function.Body.Segment);
            }
        }

        function.Body.SetArguments(this, Parameters);

        var block = function.Body.First().GetAllNodes();
        foreach (var node in block)
        {
            if (node is ReturnStatement retStmt)
            {
                var retType = retStmt.NodeCheck();
                if (retType.Equals(function.Type.ReturnType))
                {
                    function.Body.Clear();
                    return retType;
                }

                throw new WrongReturnType(retStmt.Segment, function.Type.ReturnType, retType);
            }

            if (node.CanEvaluate && !(node is CallExpression call && call.Member.Id == Member.Id))
            {
                node.NodeCheck();
            }
        }

        function.Body.Clear();
        return TypeUtils.JavaScriptTypes.Void;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        var nodes = new List<AbstractSyntaxTreeNode>
        {
            Member
        };
        nodes.AddRange(Parameters);
        return nodes.GetEnumerator();
    }

    protected override string NodeRepresentation() => "()";

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Nodes.Statements;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public class CallExpression : LeftHandSideExpression
{
    private readonly MemberExpression _member;
    private readonly List<Expression> _expressions;

    public CallExpression(MemberExpression member, IEnumerable<Expression> expressions)
    {
        _member = member;
        _member.Parent = this;

        _expressions = new List<Expression>(expressions);
        _expressions.ForEach(expr => expr.Parent = this);
    }
    
    public override IdentifierReference Id =>
        _member.Id;

    private FunctionSymbol GetFunction()
    {
        if (_member.Any())
        {
            var table = SymbolTable.FindSymbol<ObjectSymbol>(_member.Id).Table;
            var chain = _member.AccessChain;
            while (chain.HasNext())
            {
                table = chain switch
                {
                    DotAccess dot => table.FindSymbol<ObjectSymbol>(dot.Id).Table,
                    IndexAccess => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
                chain = chain.Next;
            }

            return table.FindSymbol<FunctionSymbol>(((DotAccess) chain).Id);
        }

        return SymbolTable.FindSymbol<FunctionSymbol>(_member.Id);
    }

    internal override Type NodeCheck()
    {
        if (_member.Any())
        {
            _member.NodeCheck();
        }
        else
        {
            IdentifierReference idRef = _member.Id;
            idRef.NodeCheck();
        }

        var function = GetFunction();
        if (function == null)
        {
            throw new SymbolIsNotCallable(_member.Id, Segment);
        }

        if (!function.Type.ReturnType.Equals(TypeUtils.JavaScriptTypes.Void))
        {
            if (!function.Body.HasReturnStatement())
            {
                throw new FunctionWithoutReturnStatement(function.Body.Segment);
            }
        }

        function.Body.SetArguments(this, _expressions);

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

            if (node.CanEvaluate && !(node is CallExpression call && call._member.Id == _member.Id))
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
            _member
        };
        nodes.AddRange(_expressions);
        return nodes.GetEnumerator();
    }

    protected override string NodeRepresentation() => "()";

    /*private List<Instruction> Print(int start)
    {
        var instructions = new List<Instruction>();
        var expression = _expressions.First();
        if (expression is not PrimaryExpression primaryExpression)
        {
            instructions.AddRange(expression.ToInstructions(start, "_t"));
            instructions.Add(new Print(
                instructions.Last().Number + 1,
                new Name(instructions.OfType<Simple>().Last().Left)
            ));
        }
        else
        {
            instructions.Add(new Print(start, primaryExpression.ToValue()));
        }

        return instructions;
    }

    public List<Instruction> ToInstructions(int start)
    {
        return _ident.Id switch
        {
            "print" when !_ident.Any() => Print(start),
            _ => ToInstructions(start, null)
        };
    }

    public List<Instruction> ToInstructions(int start, string temp)
    {
        var instructions = new List<Instruction>();
        FunctionSymbol function;
        if (!_ident.Any())
        {
            function = SymbolTable.FindSymbol<FunctionSymbol>(_ident.Id);
        }
        else
        {
            function = GetFunction();
            instructions.AddRange(_ident.ToInstructions(start, temp));
            function.CallInfo.MethodOf = instructions.Any()
                ? instructions.OfType<Simple>().Last().Left
                : function.CallInfo.MethodOf;
            instructions.Add(
                new PushParameter(
                    start + instructions.Count,
                    "this", new Name(function.CallInfo.MethodOf))
            );
        }
        if (function.Body.First().Any())
        {
            _expressions.Zip(function.Parameters).ToList<(Expression expr, Symbol param)>()
                .ForEach(item =>
                {
                    var (expr, symbol) = item;
                    var paramInstructions = expr is PrimaryExpression
                        ? new List<Instruction>()
                        : expr.ToInstructions(start, "_t");
                    var pushNumber = start + instructions.Count + paramInstructions.Count;
                    var pushValue = expr is PrimaryExpression expression
                        ? expression.ToValue()
                        : new Name(paramInstructions.OfType<Simple>().Last().Left);
                    paramInstructions.Add(
                        new PushParameter(pushNumber, symbol.Id, pushValue)
                    );
                    instructions.AddRange(paramInstructions);
                });
            var left = temp != null
                ? temp + (start + instructions.Count)
                : null;
            instructions.Add(
                new CallFunction(
                    function.CallInfo,
                    start + instructions.Count,
                    function.Parameters.Count, left
                ));
        }

        return instructions;
    }*/

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor)
    {
        throw new NotImplementedException();
    }
}
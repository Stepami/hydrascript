using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Nodes.Statements;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM.Values;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class CallExpression : Expression
    {
        private readonly MemberExpression _ident;
        private readonly List<Expression> _expressions;

        public CallExpression(MemberExpression ident, IEnumerable<Expression> expressions)
        {
            _ident = ident;
            _ident.Parent = this;

            _expressions = new List<Expression>(expressions);
            _expressions.ForEach(expr => expr.Parent = this);
        }

        private FunctionSymbol GetFunction()
        {
            if (_ident.Any())
            {
                var table = SymbolTable.FindSymbol<ObjectSymbol>(_ident.Id).Table;
                var chain = _ident.AccessChain;
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

            return SymbolTable.FindSymbol<FunctionSymbol>(_ident.Id);
        }

        internal override Type NodeCheck()
        {
            if (_ident.Any())
            {
                _ident.NodeCheck();
            }
            else
            {
                IdentifierReference idRef = _ident;
                idRef.NodeCheck();
            }

            var function = GetFunction();
            if (function == null)
            {
                throw new SymbolIsNotCallable(_ident.Id, Segment);
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

                if (node.CanEvaluate && !(node is CallExpression call && call._ident.Id == _ident.Id))
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
                _ident
            };
            nodes.AddRange(_expressions);
            return nodes.GetEnumerator();
        }

        protected override string NodeRepresentation() => "()";

        private List<Instruction> Print(int start)
        {
            var instructions = new List<Instruction>();
            var expression = _expressions.First();
            if (!expression.Primary())
            {
                instructions.AddRange(expression.ToInstructions(start, "_t"));
                instructions.Add(new Print(
                    instructions.Last().Number + 1,
                    new Name(instructions.OfType<Simple>().Last().Left)
                ));
            }
            else
            {
                instructions.Add(new Print(start, ((PrimaryExpression) expression).ToValue()));
            }

            return instructions;
        }

        public override List<Instruction> ToInstructions(int start)
        {
            return _ident.Id switch
            {
                "print" when !_ident.Any() => Print(start),
                _ => ToInstructions(start, null)
            };
        }

        public override List<Instruction> ToInstructions(int start, string temp)
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
                    : _ident.Id;
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
                        var paramInstructions = expr.Primary()
                            ? new List<Instruction>()
                            : expr.ToInstructions(start, "_t");
                        var pushNumber = start + instructions.Count + paramInstructions.Count;
                        var pushValue = expr.Primary()
                            ? ((PrimaryExpression) expr).ToValue()
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
        }
    }
}
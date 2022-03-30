using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Nodes.Statements;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM;

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

        internal override Type NodeCheck()
        {
            var function = SymbolTable.FindSymbol<FunctionSymbol>(_ident.Id);
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

                if (node.CanEvaluate)
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
                instructions.Add(new PrintInstruction(
                    instructions.Last().Number + 1,
                    new Name(instructions.OfType<ThreeAddressCodeInstruction>().Last().Left)
                ));
            }
            else
            {
                instructions.Add(new PrintInstruction(start, ((PrimaryExpression) expression).ToValue()));
            }

            return instructions;
        }

        private List<Instruction> CastToString(int start)
        {
            var instructions = new List<Instruction>();
            var expression = _expressions.First();
            var castNumber = start;

            if (!expression.Primary())
            {
                instructions.AddRange(expression.ToInstructions(start, "_t"));
                castNumber = instructions.Last().Number + 1;
            }

            instructions.Add(new AsStringInstruction(
                "_t" + castNumber,
                expression.Primary()
                    ? ((PrimaryExpression) expression).ToValue()
                    : new Name(instructions.OfType<ThreeAddressCodeInstruction>().Last().Left),
                castNumber
            ));

            return instructions;
        }

        public override List<Instruction> ToInstructions(int start)
        {
            return _ident.Id switch
            {
                "print" => Print(start),
                "toString" => CastToString(start),
                _ => ToInstructions(start, null)
            };
        }

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            if (_ident.Id == "toString")
            {
                return CastToString(start);
            }

            var instructions = new List<Instruction>();
            var function = SymbolTable.FindSymbol<FunctionSymbol>(_ident.Id);
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
                            : new Name(paramInstructions.OfType<ThreeAddressCodeInstruction>().Last().Left);
                        paramInstructions.Add(
                            new PushParameterInstruction(pushNumber, symbol.Id, pushValue)
                        );
                        instructions.AddRange(paramInstructions);
                    });
                var left = temp != null
                    ? temp + (start + instructions.Count)
                    : null;
                instructions.Add(
                    new CallInstruction(
                        function.CallInfo,
                        start + instructions.Count,
                        function.Parameters.Count, left
                    ));
            }

            return instructions;
        }
    }
}
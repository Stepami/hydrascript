using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Type = Interpreter.Lib.IR.CheckSemantics.Types.Type;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions
{
    public class AssignmentExpression : Expression
    {
        private readonly MemberExpression _destination;
        private readonly Expression _source;
        private readonly Type _destinationType;

        public AssignmentExpression(MemberExpression destination, Expression source, Type destinationType = null)
        {
            _destination = destination;
            destination.Parent = this;

            _source = source;
            source.Parent = this;

            _destinationType = destinationType;
        }

        internal override Type NodeCheck()
        {
            var id = _destination.Id;
            var type = _source.NodeCheck();
            if (Parent is LexicalDeclaration declaration)
            {
                if (declaration.Readonly && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
                {
                    throw new ConstWithoutInitializer(_destination);
                }

                if (SymbolTable.ContainsSymbol(_destination.Id))
                {
                    throw new DeclarationAlreadyExists(_destination);
                }

                if (_destinationType != null && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
                {
                    type = _destinationType;
                }

                if (_destinationType != null && !_destinationType.Equals(type))
                {
                    throw new IncompatibleTypesOfOperands(Segment, _destinationType, type);
                }

                if (_destinationType == null && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
                {
                    throw new CannotDefineType(Segment);
                }

                var typeOfSymbol = _destinationType != null && type.Equals(TypeUtils.JavaScriptTypes.Undefined)
                    ? _destinationType
                    : type;
                if (typeOfSymbol is ObjectType objectTypeOfSymbol)
                {
                    SymbolTable.AddSymbol(new ObjectSymbol(id, objectTypeOfSymbol, declaration.Readonly, _source.SymbolTable)
                    {
                        Table = _source.SymbolTable
                    });
                }
                else
                {
                    SymbolTable.AddSymbol(new VariableSymbol(id, typeOfSymbol, declaration.Readonly));
                }
            }
            else
            {
                var symbol = SymbolTable.FindSymbol<VariableSymbol>(id);
                if (symbol != null)
                {
                    if (symbol.ReadOnly)
                    {
                        throw new AssignmentToConst(_destination);
                    }

                    if (!_destination.NodeCheck().Equals(type))
                    {
                        throw new IncompatibleTypesOfOperands(Segment, symbol.Type, type);
                    }
                }
            }

            return type;
        }

        public override List<Instruction> ToInstructions(int start)
        {
            var instructions = new List<Instruction>();
            var destInstructions = _destination.ToInstructions(start, _destination.Id);
            var srcInstructions = _source.ToInstructions(start + destInstructions.Count, _destination.Id);

            instructions.AddRange(destInstructions);
            instructions.AddRange(srcInstructions);
            start += instructions.Count;

            if (_source is MemberExpression member && member.Any())
            {
                var access = (member.First() as AccessExpression)?.Tail;
                var dest = destInstructions.Any()
                    ? destInstructions.OfType<Simple>().Last().Left
                    : _destination.Id;
                var src = srcInstructions.Any()
                    ? srcInstructions.OfType<Simple>().Last().Left
                    : member.Id;
                var instruction = access switch
                {
                    DotAccess dot => new Simple(dest, (new Name(src), new Constant(dot.Id, dot.Id)), ".", start),
                    IndexAccess index => new Simple(dest, (new Name(src), index.Expression.ToValue()), "[]", start),
                    _ => throw new NotImplementedException()
                };
                instructions.Add(instruction);
                start++;
            }

            var last = instructions.OfType<Simple>().LastOrDefault();
            if (last != null)
            {
                if (_source is AssignmentExpression)
                {
                    instructions.Add(new Simple(
                        _destination.Id,
                        (null, new Name(last.Left)),
                        "", last.Jump()
                    ));
                    start++;
                }
                else
                {
                    last.Left = _destination.Id;
                }
            }

            if (_destination.Any())
            {
                var access = (_destination.First() as AccessExpression)?.Tail;
                var lastIndex = instructions.Count - 1;
                last = instructions.OfType<Simple>().Last();
                if (last.Assignment)
                {
                    instructions.RemoveAt(lastIndex);
                    start--;
                }
                else
                {
                    last.Left = "_t" + last.Number;
                }

                var dest = destInstructions.Any()
                    ? destInstructions.OfType<Simple>().Last().Left
                    : _destination.Id;
                var src = !last.Assignment
                    ? new Name(last.Left)
                    : last.Source;
                Instruction instruction = access switch
                {
                    DotAccess dot => new DotAssignment(dest, (new Constant(dot.Id, @$"\""{dot.Id}\"""), src), start),
                    IndexAccess index => new IndexAssignment(dest, (index.Expression.ToValue(), src), start),
                    _ => throw new NotImplementedException()
                };
                instructions.Add(instruction);
            }

            return instructions;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _destination;
            yield return _source;
        }

        protected override string NodeRepresentation() => "=";

        public override List<Instruction> ToInstructions(int start, string temp) => ToInstructions(start);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM.Values;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
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
                if (declaration.Const() && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
                {
                    throw new ConstWithoutInitializer(_destination);
                }

                if (SymbolTable.FindSymbol<Symbol>(_destination.Id) != null)
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

                SymbolTable.AddSymbol(new VariableSymbol(id, declaration.Const())
                {
                    Type = _destinationType != null && type.Equals(TypeUtils.JavaScriptTypes.Undefined)
                        ? _destinationType
                        : type
                });
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
                    IndexAccess => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
                instructions.Add(instruction);
                start++;
            }
            
            var last = instructions.OfType<Simple>().Last();
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
                    IndexAccess => throw new NotImplementedException(),
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
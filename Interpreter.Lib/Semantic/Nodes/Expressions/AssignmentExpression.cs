using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM;
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

                if (_destinationType != null && !_destinationType.Equals(type))
                {
                    throw new IncompatibleTypesOfOperands(Segment, _destinationType, type);
                }

                SymbolTable.AddSymbol(new VariableSymbol(id, declaration.Const())
                {
                    Type = type
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

                    if (!symbol.Type.Equals(type))
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
            instructions.AddRange(_source.ToInstructions(start, _destination.Id));
            if (_source.Primary()) return instructions;
            var last = instructions.OfType<Simple>().Last();
            if (_source is AssignmentExpression)
            {
                instructions.Add(new Simple(
                    _destination.Id,
                    (null, new Name(last.Left)),
                    "", last.Jump()
                ));
            }
            else
            {
                last.Left = _destination.Id;
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
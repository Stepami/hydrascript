using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class AssignmentExpression : Expression
    {
        private readonly MemberExpression destination;
        private readonly Expression source;
        private readonly Type destinationType;

        public AssignmentExpression(MemberExpression destination, Expression source, Type destinationType = null)
        {
            this.destination = destination;
            destination.Parent = this;

            this.source = source;
            source.Parent = this;

            this.destinationType = destinationType;
        }

        internal override Type NodeCheck()
        {
            var id = destination.Id;
            var type = source.NodeCheck();
            if (Parent is LexicalDeclaration declaration)
            {
                if (declaration.Const() && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
                {
                    throw new ConstWithoutInitializer(destination);
                }

                if (SymbolTable.FindSymbol<Symbol>(destination.Id) != null)
                {
                    throw new DeclarationAlreadyExists(destination);
                }

                if (destinationType != null && !destinationType.Equals(type))
                {
                    throw new IncompatibleTypesOfOperands(Segment, destinationType, type);
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
                        throw new AssignmentToConst(destination);
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
            instructions.AddRange(source.ToInstructions(start, destination.Id));
            if (source.Primary()) return instructions;
            var last = instructions.OfType<ThreeAddressCodeInstruction>().Last();
            if (source is AssignmentExpression)
            {
                instructions.Add(new ThreeAddressCodeInstruction(
                    destination.Id,
                    (null, new Name(last.Left)),
                    "", last.Jump()
                ));
            }
            else
            {
                last.Left = destination.Id;
            }
            return instructions;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return destination;
            yield return source;
        }

        protected override string NodeRepresentation() => "=";

        public override List<Instruction> ToInstructions(int start, string temp) => ToInstructions(start);
    }
}
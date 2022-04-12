using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class MemberExpression : Expression
    {
        private readonly IdentifierReference _id;
        private readonly AccessExpression _accessChain;

        public MemberExpression(IdentifierReference id, AccessExpression accessChain)
        {
            _id = id;
            _id.Parent = this;
            
            _accessChain = accessChain;
            if (accessChain != null)
            {
                _accessChain.Parent = this;
            }
        }

        public string Id => _id.Id;

        internal override Type NodeCheck()
        {
            if (_accessChain == null)
            {
                return _id.NodeCheck();
            }

            var symbol = SymbolTable.FindSymbol<VariableSymbol>(_id.Id);
            if (symbol == null)
            {
                throw new UnknownIdentifierReference(_id);
            }

            return _accessChain.Check(symbol.Type);
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            if (_accessChain != null)
            {
                yield return _accessChain;
            }
        }

        protected override string NodeRepresentation() => Id;

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            if (_accessChain != null && _accessChain.HasNext())
            {
                return _accessChain.ToInstructions(start, temp);
            }

            return new();
        }

        public static implicit operator IdentifierReference(MemberExpression member) => 
            member._id;
        
        public static explicit operator MemberExpression(IdentifierReference idRef) =>
            new (idRef, null);
    }
}
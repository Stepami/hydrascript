using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class MemberExpression : Expression
    {
        private readonly IdentifierReference id;
        private readonly AccessExpression accessChain;

        public MemberExpression(IdentifierReference id, AccessExpression accessChain)
        {
            this.id = id;
            this.id.Parent = this;
            
            this.accessChain = accessChain;
            if (accessChain != null)
            {
                this.accessChain.Parent = this;
            }
        }

        public string Id => id.Id;
        
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            if (accessChain != null)
            {
                yield return accessChain;
            }
        }

        protected override string NodeRepresentation() => Id;

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }

        public static implicit operator IdentifierReference(MemberExpression member) => 
            member.id;
        
        public static explicit operator MemberExpression(IdentifierReference idRef) =>
            new (idRef, null);
    }
}
using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Nodes.Declarations
{
    public class LexicalDeclaration : Declaration
    {
        private readonly DeclarationType _declarationType;
        private readonly List<AssignmentExpression> _assignments = new();

        public LexicalDeclaration(bool readOnly)
        {
            _declarationType = readOnly ? DeclarationType.Const : DeclarationType.Let;
        }

        public void AddAssignment(string id, Segment identSegment, Expression expression, Segment assignSegment = null, Type destinationType = null)
        {
            var identRef = new IdentifierReference(id)
            {
                SymbolTable = SymbolTable, 
                Segment = identSegment
            };
            var assignment =
                new AssignmentExpression(
                    new MemberExpression(identRef, null),
                    expression,
                    destinationType
                )
                {
                    SymbolTable = SymbolTable,
                    Segment = assignSegment,
                    Parent = this
                };
            _assignments.Add(assignment);
        }

        public bool Const() => _declarationType == DeclarationType.Const;

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() => _assignments.GetEnumerator();

        protected override string NodeRepresentation() => _declarationType.ToString();

        public override List<Instruction> ToInstructions(int start)
        {
            var instructions = new List<Instruction>();
            var offset = start;
            foreach (var aInstructions in _assignments.Select(assignment => assignment.ToInstructions(offset)))
            {
                instructions.AddRange(aInstructions);
                offset += aInstructions.Count;
            }

            return instructions;
        }

        private enum DeclarationType
        {
            Let,
            Const
        }
    }
}
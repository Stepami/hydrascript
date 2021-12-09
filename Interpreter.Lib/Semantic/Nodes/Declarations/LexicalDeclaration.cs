using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.Semantic.Nodes.Expressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Nodes.Declarations
{
    public class LexicalDeclaration : Declaration
    {
        private readonly DeclarationType declarationType;
        private readonly List<AssignmentExpression> assignments = new();

        public LexicalDeclaration(bool readOnly)
        {
            declarationType = readOnly ? DeclarationType.Const : DeclarationType.Let;
        }

        public void AddAssignment(string id, Segment identSegment, Expression expression)
        {
            var assignment =
                new AssignmentExpression(
                    new IdentifierReference(id) {SymbolTable = SymbolTable, Segment = identSegment},
                    expression
                )
                {
                    SymbolTable = SymbolTable,
                    Parent = this
                };
            assignments.Add(assignment);
        }

        public bool Const() => declarationType == DeclarationType.Const;

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() => assignments.GetEnumerator();

        protected override string NodeRepresentation() => declarationType.ToString();

        public override List<Instruction> ToInstructions(int start)
        {
            var instructions = new List<Instruction>();
            var offset = start;
            foreach (var aInstructions in assignments.Select(assignment => assignment.ToInstructions(offset)))
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
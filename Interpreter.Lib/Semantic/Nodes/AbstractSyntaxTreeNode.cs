using System.Collections;
using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Nodes.Visitors;
using Interpreter.Lib.Semantic.Types;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.Semantic.Nodes
{
    public abstract class AbstractSyntaxTreeNode : 
        IEnumerable<AbstractSyntaxTreeNode>,
        IVisitable<InstructionProvider, List<Instruction>>,
        IVisitable<SemanticChecker, Type>
    {
        public AbstractSyntaxTreeNode Parent { get; set; }
        
        public SymbolTable SymbolTable { get; set; }

        public bool CanEvaluate { get; protected init; }
        
        public Segment Segment { get; init; }

        protected AbstractSyntaxTreeNode()
        {
            Parent = null;
            CanEvaluate = false;
        }

        internal List<AbstractSyntaxTreeNode> GetAllNodes()
        {
            var result = new List<AbstractSyntaxTreeNode>
            {
                this
            };
            foreach (var child in this)
            {
                result.AddRange(child.GetAllNodes());
            }

            return result;
        }

        public bool ChildOf<T>() where T : AbstractSyntaxTreeNode
        {
            var parent = Parent;
            while (parent != null)
            {
                if (parent is T)
                {
                    return true;
                }
                parent = parent.Parent;
            }

            return false;
        }

        public void SemanticCheck()
        {
            if (CanEvaluate && !ChildOf<FunctionDeclaration>())
            {
                NodeCheck();
            }
        }

        internal virtual Type NodeCheck() => null;

        public virtual List<Instruction> ToInstructions(int start) => new ();

        public abstract IEnumerator<AbstractSyntaxTreeNode> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected abstract string NodeRepresentation();

        public abstract List<Instruction> Accept(InstructionProvider visitor);

        public abstract Type Accept(SemanticChecker visitor);

        public override string ToString() => $"{GetHashCode()} [label=\"{NodeRepresentation()}\"]";
    }
}
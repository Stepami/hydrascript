using System.Collections;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Variables;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Nodes;

public abstract class AbstractSyntaxTreeNode :
    IEnumerable<AbstractSyntaxTreeNode>,
    IVisitable<InstructionProvider, AddressedInstructions>
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

    public abstract AddressedInstructions Accept(InstructionProvider visitor);

    public override string ToString() => $"{GetHashCode()} [label=\"{NodeRepresentation()}\"]";
}
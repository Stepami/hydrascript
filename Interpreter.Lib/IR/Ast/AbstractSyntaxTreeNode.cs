using System.Collections;
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Variables;
using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast;

public abstract class AbstractSyntaxTreeNode : IEnumerable<AbstractSyntaxTreeNode>,
    IVisitable<InstructionProvider, AddressedInstructions>,
    IVisitable<SemanticChecker, Type>,
    IVisitable<SymbolTableInitializer>,
    IVisitable<TypeSystemLoader>,
    IVisitable<DeclarationVisitor>
{
    public AbstractSyntaxTreeNode Parent { get; set; }

    public SymbolTable SymbolTable { get; set; }

    public Segment Segment { get; init; }

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

    public abstract IEnumerator<AbstractSyntaxTreeNode> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    #region Visitors

    public virtual Unit Accept(SymbolTableInitializer visitor) =>
        visitor.Visit(this);

    public virtual Unit Accept(TypeSystemLoader visitor) =>
        visitor.Visit(this);
    
    public virtual Unit Accept(DeclarationVisitor visitor) =>
        visitor.Visit(this);

    public virtual Type Accept(SemanticChecker visitor) =>
        "undefined";

    public abstract AddressedInstructions Accept(InstructionProvider visitor);

    #endregion
    
    protected abstract string NodeRepresentation();
    
    public override string ToString() =>
        $"{GetHashCode()} [label=\"{NodeRepresentation()}\"]";
}
namespace Interpreter.Lib.Semantic.Nodes.Declarations
{
    public abstract class Declaration : StatementListItem
    {
        public override bool IsDeclaration() => true;
    }
}
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public class AssignmentExpression : Expression
{
    public LeftHandSideExpression Destination { get; }
    public Expression Source { get; }
    public Type DestinationType { get; }

    public AssignmentExpression(LeftHandSideExpression lhs, Expression source, Type destinationType = null)
    {
        Destination = lhs;
        lhs.Parent = this;

        Source = source;
        source.Parent = this;

        DestinationType = destinationType;
    }

    internal override Type NodeCheck()
    {
        var id = Destination.Id;
        var type = Source.NodeCheck();
        if (Parent is LexicalDeclaration declaration)
        {
            if (declaration.Readonly && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
            {
                throw new ConstWithoutInitializer(Destination.Id);
            }

            if (SymbolTable.ContainsSymbol(Destination.Id))
            {
                throw new DeclarationAlreadyExists(Destination.Id);
            }

            if (DestinationType != null && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
            {
                type = DestinationType;
            }

            if (DestinationType != null && !DestinationType.Equals(type))
            {
                throw new IncompatibleTypesOfOperands(Segment, DestinationType, type);
            }

            if (DestinationType == null && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
            {
                throw new CannotDefineType(Segment);
            }

            var typeOfSymbol = DestinationType != null && type.Equals(TypeUtils.JavaScriptTypes.Undefined)
                ? DestinationType
                : type;
            if (typeOfSymbol is ObjectType objectTypeOfSymbol)
            {
                SymbolTable.AddSymbol(new ObjectSymbol(id, objectTypeOfSymbol, declaration.Readonly, Source.SymbolTable)
                {
                    Table = Source.SymbolTable
                });
            }
            else
            {
                SymbolTable.AddSymbol(new VariableSymbol(id, typeOfSymbol, declaration.Readonly));
            }
        }
        else
        {
            var symbol = SymbolTable.FindSymbol<VariableSymbol>(id);
            if (symbol != null)
            {
                if (symbol.ReadOnly)
                {
                    throw new AssignmentToConst(Destination.Id);
                }

                if (!Destination.NodeCheck().Equals(type))
                {
                    throw new IncompatibleTypesOfOperands(Segment, symbol.Type, type);
                }
            }
        }

        return type;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Destination;
        yield return Source;
    }

    protected override string NodeRepresentation() => "=";

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}
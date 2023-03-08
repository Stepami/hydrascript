using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public class AssignmentExpression : Expression
{
    public MemberExpression Destination { get; }
    public Expression Source { get; }
    private readonly Type _destinationType;

    public AssignmentExpression(MemberExpression destination, Expression source, Type destinationType = null)
    {
        Destination = destination;
        destination.Parent = this;

        Source = source;
        source.Parent = this;

        _destinationType = destinationType;
    }

    internal override Type NodeCheck()
    {
        var id = Destination.Id;
        var type = Source.NodeCheck();
        if (Parent is LexicalDeclaration declaration)
        {
            if (declaration.Readonly && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
            {
                throw new ConstWithoutInitializer(Destination);
            }

            if (SymbolTable.ContainsSymbol(Destination.Id))
            {
                throw new DeclarationAlreadyExists(Destination);
            }

            if (_destinationType != null && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
            {
                type = _destinationType;
            }

            if (_destinationType != null && !_destinationType.Equals(type))
            {
                throw new IncompatibleTypesOfOperands(Segment, _destinationType, type);
            }

            if (_destinationType == null && type.Equals(TypeUtils.JavaScriptTypes.Undefined))
            {
                throw new CannotDefineType(Segment);
            }

            var typeOfSymbol = _destinationType != null && type.Equals(TypeUtils.JavaScriptTypes.Undefined)
                ? _destinationType
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
                    throw new AssignmentToConst(Destination);
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
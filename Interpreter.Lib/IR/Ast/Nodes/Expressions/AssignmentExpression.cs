using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public class AssignmentExpression : Expression
{
    public MemberExpression Destination { get; }
    private readonly Expression _source;
    private readonly Type _destinationType;

    public AssignmentExpression(MemberExpression destination, Expression source, Type destinationType = null)
    {
        Destination = destination;
        destination.Parent = this;

        _source = source;
        source.Parent = this;

        _destinationType = destinationType;
    }

    internal override Type NodeCheck()
    {
        var id = Destination.Id;
        var type = _source.NodeCheck();
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
                SymbolTable.AddSymbol(new ObjectSymbol(id, objectTypeOfSymbol, declaration.Readonly, _source.SymbolTable)
                {
                    Table = _source.SymbolTable
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

    public override List<Instruction> ToInstructions(int start)
    {
        var instructions = new List<Instruction>();
        var destInstructions = Destination.ToInstructions(start, Destination.Id);
        var srcInstructions = _source.ToInstructions(start + destInstructions.Count, Destination.Id);

        instructions.AddRange(destInstructions);
        instructions.AddRange(srcInstructions);
        start += instructions.Count;

        if (_source is MemberExpression member && member.Any())
        {
            var access = (member.First() as AccessExpression)?.Tail;
            var dest = destInstructions.Any()
                ? destInstructions.OfType<Simple>().Last().Left
                : Destination.Id;
            var src = srcInstructions.Any()
                ? srcInstructions.OfType<Simple>().Last().Left
                : member.Id;
            var instruction = access switch
            {
                DotAccess dot => new Simple(dest, (new Name(src), new Constant(dot.Id, dot.Id)), ".", start),
                IndexAccess index => new Simple(dest, (new Name(src), index.Expression.ToValue()), "[]", start),
                _ => throw new NotImplementedException()
            };
            instructions.Add(instruction);
            start++;
        }

        var last = instructions.OfType<Simple>().LastOrDefault();
        if (last != null)
        {
            if (_source is AssignmentExpression)
            {
                instructions.Add(new Simple(
                    Destination.Id,
                    (null, new Name(last.Left)),
                    "", last.Jump()
                ));
                start++;
            }
            else
            {
                last.Left = Destination.Id;
            }
        }

        if (Destination.Any())
        {
            var access = (Destination.First() as AccessExpression)?.Tail;
            var lastIndex = instructions.Count - 1;
            last = instructions.OfType<Simple>().Last();
            if (last.Assignment)
            {
                instructions.RemoveAt(lastIndex);
                start--;
            }
            else
            {
                last.Left = "_t" + last.Number;
            }

            var dest = destInstructions.Any()
                ? destInstructions.OfType<Simple>().Last().Left
                : Destination.Id;
            var src = !last.Assignment
                ? new Name(last.Left)
                : last.Source;
            Instruction instruction = access switch
            {
                DotAccess dot => new DotAssignment(dest, (new Constant(dot.Id, @$"\""{dot.Id}\"""), src), start),
                IndexAccess index => new IndexAssignment(dest, index.Expression.ToValue(), src, start),
                _ => throw new NotImplementedException()
            };
            instructions.Add(instruction);
        }

        return instructions;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Destination;
        yield return _source;
    }

    protected override string NodeRepresentation() => "=";

    public override List<Instruction> ToInstructions(int start, string temp) => ToInstructions(start);
}
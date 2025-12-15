using Cysharp.Text;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;

public class Simple : Instruction
{
    public Name? Left { get; set; }

    protected readonly (IValue? left, IValue? right) Right;
    private readonly string _operator = string.Empty;

    protected Simple(Name? left) => Left = left;

    public Simple(
        Name? left,
        (IValue? left, IValue? right) right,
        string @operator)
    {
        Left = left;
        Right = right;
        _operator = @operator;
    }

    public Simple(IValue value) : this(
        left: null,
        right: (null, value),
        @operator: string.Empty)
    {
    }

    public Simple(string unaryOperator, IValue value) : this(
        left: null,
        right: (null, value),
        @operator: unaryOperator)
    {
    }

    public Simple(
        IValue leftValue,
        string binaryOperator,
        IValue rightValue) :
        this(
            left: null,
            right: (leftValue, rightValue),
            @operator: binaryOperator)
    {
    }

    protected override void OnSetOfAddress(IAddress address) =>
        Left ??= new Name(address.Name);

    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        Left?.Set(frame, Right.left is null ? GetUnaryResult(frame) : GetBinaryResult(frame));

        return Address.Next;
    }

    private object? GetUnaryResult(Frame frame)
    {
        var value = Right.right!.Get(frame);
        return _operator switch
        {
            "-" => -Convert.ToDouble(value),
            "!" => !Convert.ToBoolean(value),
            "~" => ((List<object>)value!).Count,
            "" => value,
            _ => throw new NotSupportedException($"_operator {_operator} is not supported")
        };
    }

    private object? GetBinaryResult(Frame frame)
    {
        object? lValue = Right.left!.Get(frame), rValue = Right.right!.Get(frame);
        return _operator switch
        {
            "+" when lValue is string => ZString.Concat(lValue, rValue),
            "+" => Convert.ToDouble(lValue) + Convert.ToDouble(rValue),
            "-" => Convert.ToDouble(lValue) - Convert.ToDouble(rValue),
            "*" => Convert.ToDouble(lValue) * Convert.ToDouble(rValue),
            "/" => Convert.ToDouble(lValue) / Convert.ToDouble(rValue),
            "%" => Convert.ToDouble(lValue) % Convert.ToDouble(rValue),
            "||" => Convert.ToBoolean(lValue) || Convert.ToBoolean(rValue),
            "&&" => Convert.ToBoolean(lValue) && Convert.ToBoolean(rValue),
            "==" => Equals(lValue, rValue),
            "!=" => !Equals(lValue, rValue),
            ">" => Convert.ToDouble(lValue) > Convert.ToDouble(rValue),
            ">=" => Convert.ToDouble(lValue) >= Convert.ToDouble(rValue),
            "<" => Convert.ToDouble(lValue) < Convert.ToDouble(rValue),
            "<=" => Convert.ToDouble(lValue) <= Convert.ToDouble(rValue),
            "." => ((Dictionary<string, object>)lValue!)[rValue!.ToString()!],
            "[]" => ((List<object>)lValue!)[Convert.ToInt32(rValue)],
            "++" => ((List<object>)lValue!).Concat((List<object>)rValue!).ToList(),
            _ => throw new NotSupportedException($"_operator {_operator} is not supported")
        };
    }

    protected override string ToStringInternal() =>
        Right.left == null
            ? $"{Left} = {_operator}{Right.right}"
            : $"{Left} = {Right.left} {_operator} {Right.right}";
}
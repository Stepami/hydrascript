using Cysharp.Text;
using HydraScript.Domain.BackEnd.Impl.Frames;
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
        Left ??= new Name(address.Name, Name.NullFrameInstance);

    public override IAddress? Execute(IExecuteParams executeParams)
    {
        Left?.SetFrame(new CurrentFrame(executeParams.FrameContext));
        Assign();
        return Address.Next;
    }

    /// <summary>
    /// Совершить "присваивание" результата в <see cref="Left"/>
    /// </summary>
    protected virtual void Assign()
    {
        Left?.Set(Right.left is null ? GetUnaryResult() : GetBinaryResult());
    }

    private object? GetUnaryResult()
    {
        var value = Right.right!.Get();
        return _operator switch
        {
            "-" => -Convert.ToDouble(value),
            "!" => !Convert.ToBoolean(value),
            "~" => ((List<object>)value!).Count,
            "" => value,
            _ => throw new NotSupportedException($"_operator {_operator} is not supported")
        };
    }

    private object? GetBinaryResult()
    {
        object? lValue = Right.left!.Get(), rValue = Right.right!.Get();
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
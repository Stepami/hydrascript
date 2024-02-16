using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions.WithAssignment;

public class Simple : Instruction
{
    public string Left { get; set; }

    protected readonly (IValue left, IValue right) Right;
    private readonly string _operator;

    protected Simple(string left) => Left = left;

    public Simple(
        string left,
        (IValue left, IValue right) right,
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
        Left ??= $"_t{unchecked((uint)address.GetHashCode())}";

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        if (Right.left == null)
        {
            var value = Right.right.Get(frame);
            frame[Left] = _operator switch
            {
                "-" => -Convert.ToDouble(value),
                "!" => !Convert.ToBoolean(value),
                "~" => ((List<object>)value).Count,
                "" => value,
                _ => throw new NotSupportedException($"_operator {_operator} is not supported")
            };
        }
        else
        {
            object lValue = Right.left.Get(frame), rValue = Right.right.Get(frame);
            frame[Left] = _operator switch
            {
                "+" when lValue is string => lValue.ToString() + rValue,
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
                "." => ((Dictionary<string, object>)lValue)[rValue.ToString()!],
                "[]" => ((List<object>)lValue)[Convert.ToInt32(rValue)],
                "++" => ((List<object>)lValue).Concat((List<object>)rValue).ToList(),
                _ => throw new NotSupportedException($"_operator {_operator} is not supported")
            };
        }

        if (vm.CallStack.Any())
        {
            var call = vm.CallStack.Peek();
            var methodOf = call.To.MethodOf;
            if (methodOf != null)
            {
                var methodOwner = (Dictionary<string, object>)frame[methodOf];
                if (methodOwner.ContainsKey(Left))
                {
                    methodOwner[Left] = frame[Left];
                }
            }
        }

        return Address.Next;
    }

    protected override string ToStringInternal() =>
        Right.left == null
            ? $"{Left} = {_operator}{Right.right}"
            : $"{Left} = {Right.left} {_operator} {Right.right}";
}
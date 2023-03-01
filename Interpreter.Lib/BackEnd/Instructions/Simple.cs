using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class Simple : Instruction
{
    public string Left { get; set; }

    protected (IValue left, IValue right) right;
    protected readonly string @operator;

    public Simple(string left,
        (IValue left, IValue right) right,
        string @operator)
    {
        Left = left;
        this.right = right;
        this.@operator = @operator;
    }

    public Simple(IValue value) : this(
        left: string.Empty,
        right: (null, value),
        @operator: string.Empty
    ) { }

    public IValue Source =>
        right.right;

    public bool Assignment => @operator == "";

    protected override void OnSetOfAddress(IAddress address) =>
        Left = $"_t{address.GetHashCode()}";

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        if (right.left == null)
        {
            var value = right.right.Get(frame);
            frame[Left] = @operator switch
            {
                "-" => -Convert.ToDouble(value),
                "!" => !Convert.ToBoolean(value),
                "~" => ((List<object>) value).Count,
                "" => value,
                _ => throw new NotImplementedException()
            };
        }
        else
        {
            object lValue = right.left.Get(frame), rValue = right.right.Get(frame);
            frame[Left] = @operator switch
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
                "." => ((Dictionary<string, object>) lValue)[rValue.ToString()!],
                "[]" => ((List<object>) lValue)[Convert.ToInt32(rValue)],
                "++" => ((List<object>) lValue).Concat((List<object>) rValue).ToList(),
                _ => throw new NotImplementedException()
            };
        }
        if (vm.CallStack.Any())
        {
            var call = vm.CallStack.Peek();
            var methodOf = call.To.MethodOf;
            if (methodOf != null)
            {
                var methodOwner = (Dictionary<string, object>) frame[methodOf];
                if (methodOwner.ContainsKey(Left))
                {
                    methodOwner[Left] = frame[Left];
                }
            }
        }

        return Address.Next;
    }

    protected override string ToStringInternal() => right.left == null
        ? $"{Left} = {@operator}{right.right}"
        : $"{Left} = {right.left} {@operator} {right.right}";
}
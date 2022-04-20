using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.VM;
using Interpreter.Lib.VM.Values;

namespace Interpreter.Lib.IR.Instructions
{
    public class Simple : Instruction
    {
        public string Left { get; set; }

        protected (IValue left, IValue right) right;
        protected string @operator;

        public Simple(
            string left,
            (IValue left, IValue right) right,
            string @operator,
            int number
        ) :
            base(number)
        {
            Left = left;
            this.right = right;
            this.@operator = @operator;
        }

        public IValue Source => right.right;

        public bool Assignment => @operator == "";

        public override int Execute(VirtualMachine vm)
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

            return Jump();
        }

        public void ReduceToAssignment()
        {
            right = ToStringRepresentation() switch
            {
                var s
                    when s.Contains("+ 0") || s.Contains("- 0") ||
                         s.Contains("* 1") || s.Contains("/ 1") => (null, right.left),
                var s
                    when s.Contains("0 +") || s.Contains("1 *") => (null, right.right),
                var s
                    when s.Contains("0 *") || s.Contains("0 *") => (null, new Constant(0, "0")),
                _ => throw new NotImplementedException()
            };
            @operator = "";
        }

        protected override string ToStringRepresentation() =>
            right.left == null
                ? $"{Left} = {(@operator == "" ? "" : " " + @operator)}{right.right}"
                : $"{Left} = {right.left} {@operator} {right.right}";
    }
}
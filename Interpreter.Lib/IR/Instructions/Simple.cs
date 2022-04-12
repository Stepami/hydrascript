using System;
using Interpreter.Lib.VM;
using Interpreter.Lib.VM.Values;

namespace Interpreter.Lib.IR.Instructions
{
    public class Simple : Instruction
    {
        public string Left { get; set; }

        protected readonly (IValue left, IValue right) right;
        protected readonly string @operator;

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
                    "" => value,
                    _ => frame[Left]
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
                    _ => frame[Left]
                };
            }

            return Jump();
        }

        protected override string ToStringRepresentation() =>
            right.left == null
                ? $"{Left} = {(@operator == "" ? "" : " " + @operator)}{right.right}"
                : $"{Left} = {right.left} {@operator} {right.right}";
    }
}
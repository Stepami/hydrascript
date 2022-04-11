using System;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class ThreeAddressCodeInstruction : Instruction
    {
        public string Left { get; set; }

        protected readonly (IValue left, IValue right) right;
        private readonly string _operator;

        public ThreeAddressCodeInstruction(
            string left,
            (IValue left, IValue right) right,
            string @operator,
            int number
        ) :
            base(number)
        {
            Left = left;
            this.right = right;
            _operator = @operator;
        }

        public override int Execute(VirtualMachine vm)
        {
            var frame = vm.Frames.Peek();
            if (right.left == null)
            {
                var value = right.right.Get(frame);
                frame[Left] = _operator switch
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
                    _ => frame[Left]
                };
            }

            return Jump();
        }

        protected override string ToStringRepresentation() =>
            right.left == null
                ? $"{Left} = {(_operator == "" ? "" : " " + _operator)}{right.right}"
                : $"{Left} = {right.left} {_operator} {right.right}";
    }
}
using System;

namespace Interpreter.Lib.BackEnd.Instructions
{
    public abstract class Instruction : IComparable<Instruction>
    {
        public int Number { get; }

        protected Instruction(int number) => 
            Number = number;

        public virtual int Jump() => Number + 1;

        public virtual bool End() => false;

        public abstract int Execute(VirtualMachine vm);

        public int CompareTo(Instruction other) => Number.CompareTo(other.Number);

        protected abstract string ToStringRepresentation();

        public override string ToString() => $"{Number}: {ToStringRepresentation()}";
    }
}
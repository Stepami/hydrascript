using System;
using Interpreter.Lib.BackEnd.VM;

namespace Interpreter.Lib.BackEnd.Instructions
{
    public abstract class Instruction : IComparable<Instruction>, IEquatable<Instruction>
    {
        public int Number { get; }

        public bool Leader { get; set; }

        protected Instruction(int number)
        {
            Number = number;
        }

        public virtual int Jump() => Number + 1;

        public bool Branch() => Jump() != Number + 1;

        public virtual bool End() => false;

        public abstract int Execute(VirtualMachine vm);

        public int CompareTo(Instruction other) => Number.CompareTo(other.Number);

        public bool Equals(Instruction other) => other != null && Number.Equals(other.Number);

        public override bool Equals(object obj) => Equals(obj as Instruction);

        public override int GetHashCode() => Number;

        protected abstract string ToStringRepresentation();

        public override string ToString() => $"{Number}: {ToStringRepresentation()}";
    }
}
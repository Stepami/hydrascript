using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.IR
{
    public class BasicBlock : IEnumerable<Instruction>, IEquatable<BasicBlock>, IComparable<BasicBlock>
    {
        private readonly List<Instruction> _instructions;

        public BasicBlock(IEnumerable<Instruction> instructions)
        {
            _instructions = new List<Instruction>(instructions);
            _instructions.Sort();
        }

        public int In() => _instructions.First().Number;

        public List<int> Out()
        {
            var last = _instructions.Last();
            if (last is Return ret)
            {
                return ret.ToList();
            }
            if (last.Branch() && (last is IfNotGoto))
            {
                return new()
                {
                    last.Jump(),
                    last.Number + 1
                };
            }

            return new() {last.Jump()};
        }

        public bool Equals(BasicBlock other)
        {
            if (other == null)
                return false;
            return this.Count() == other.Count() && this.Zip(other).All(pair => pair.First.Equals(pair.Second));
        }

        public IEnumerator<Instruction> GetEnumerator() => new List<Instruction>(_instructions).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int CompareTo(BasicBlock other) => In().CompareTo(other.In());

        public override bool Equals(object obj) => Equals(obj as BasicBlock);

        public override int GetHashCode() => In();

        public override string ToString()
        {
            var result = new StringBuilder($@"{GetHashCode()} [shape=box, label=""");
            result.AppendJoin("\\n", _instructions);
            return result.Append(@"""]").ToString();
        }
    }
}
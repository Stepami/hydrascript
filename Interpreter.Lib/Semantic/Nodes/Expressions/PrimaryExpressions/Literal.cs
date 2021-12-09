using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.VM;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions
{
    public class Literal : PrimaryExpression
    {
        private readonly Type type;
        private readonly object value;
        private readonly string label;

        public Literal(Type type, object value, Segment segment = null, string label = null)
        {
            this.type = type;
            this.label = label ?? value.ToString();
            this.value = value;
            Segment = segment;
        }

        internal override Type NodeCheck() => type;

        protected override string NodeRepresentation() => label;

        public override IValue ToValue() => new Constant(value, label);
    }
}
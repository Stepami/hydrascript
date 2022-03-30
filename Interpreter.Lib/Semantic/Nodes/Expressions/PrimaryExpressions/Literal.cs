using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.VM;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions
{
    public class Literal : PrimaryExpression
    {
        private readonly Type _type;
        private readonly object _value;
        private readonly string _label;

        public Literal(Type type, object value, Segment segment = null, string label = null)
        {
            _type = type;
            _label = label ?? value.ToString();
            _value = value;
            Segment = segment;
        }

        internal override Type NodeCheck() => _type;

        protected override string NodeRepresentation() => _label;

        public override IValue ToValue() => new Constant(_value, _label);
    }
}
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.IR.Optimizers;
using Interpreter.Lib.VM.Values;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class OptimizerTests
    {
        private IOptimizer<Instruction> _optimizer;

        [Fact]
        public void IdentityExpressionTests()
        {
            _optimizer = new IdentityExpression(
                new Simple(
                    "i", (new Constant(0, "0"), new Name("x")), "+", 0
                )
            );
            
            Assert.True(_optimizer.Test());
            _optimizer.Optimize();
            Assert.Equal("0: i = x", _optimizer.Instruction.ToString());
            
            
            _optimizer = new IdentityExpression(
                new Simple(
                    "i", (new Constant(1, "1"), new Name("x")), "*", 0
                )
            );
            Assert.True(_optimizer.Test());
            _optimizer.Optimize();
            Assert.Equal("0: i = x", _optimizer.Instruction.ToString());
            
            _optimizer = new IdentityExpression(
                new Simple(
                    "i", (new Constant(2, "2"), new Name("x")), "+", 0
                )
            );
            Assert.False(_optimizer.Test());

            _optimizer = new IdentityExpression(
                new Simple(
                    "i", (null, new Constant(0, "0")), "-", 0)
            );
            Assert.True(_optimizer.Test());
        }
    }
}
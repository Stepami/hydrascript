using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.IR.Optimizables;
using Interpreter.Lib.VM;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class OptimizableTests
    {
        private IOptimizable<Instruction> _optimizable;

        [Fact]
        public void IdentityExpressionTests()
        {
            _optimizable = new IdentityExpression(
                new Simple(
                    "", (new Constant(0, "0"), new Name("x")), "+", 0
                )
            );
            
            Assert.True(_optimizable.Test());
            
            _optimizable = new IdentityExpression(
                new Simple(
                    "", (new Constant(1, "1"), new Name("x")), "*", 0
                )
            );
            
            Assert.True(_optimizable.Test());
            
            _optimizable = new IdentityExpression(
                new Simple(
                    "", (new Constant(2, "2"), new Name("x")), "+", 0
                )
            );
            
            Assert.False(_optimizable.Test());
        }
    }
}
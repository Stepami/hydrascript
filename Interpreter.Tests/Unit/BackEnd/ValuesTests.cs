using Interpreter.Lib.BackEnd.Values;
using Xunit;

namespace Interpreter.Tests.Unit.BackEnd;

public class ValuesTests
{
    [Fact]
    public void ConstantNotEqualToNameTest()
    {
        var name = new Name("a");
        var constant = new Constant("a");
            
        Assert.False(name.Equals(constant));
        Assert.False(constant.Equals(name));
    }

    [Fact]
    public void ValueToStringCorrectTest()
    {
        var name = new Name("bbb");
        var constant = new Constant(1, "1.0");
            
        Assert.Equal("bbb", name.ToString());
        Assert.Equal("1.0", constant.ToString());
    }

    [Fact]
    public void NameEqualsCorrectTest()
    {
        var name1 = new Name("name");
        var name2 = new Name("name");
            
        Assert.True(name1.Equals(name2));
    }
        
    [Fact]
    public void ConstantEqualsCorrectTest()
    {
        var constant1 = new Constant(1);
        var constant2 = new Constant(1, "1.0");
            
        Assert.True(constant1.Equals(constant2));
    }
}
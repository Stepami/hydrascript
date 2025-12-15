using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.UnitTests.Domain.BackEnd;

public class ValuesTests
{
    [Fact]
    public void ConstantNotEqualToNameTest()
    {
        var name = new Name("a", Substitute.For<IFrame>());
        var constant = new Constant("a");
            
        Assert.False(name.Equals(constant));
        Assert.False(constant.Equals(name));
    }

    [Fact]
    public void ValueToStringCorrectTest()
    {
        var name = new Name("bbb", Substitute.For<IFrame>());
        var constant = new Constant(1, "1.0");
            
        Assert.Equal("bbb", name.ToString());
        Assert.Equal("1.0", constant.ToString());
    }

    [Fact]
    public void NameEqualsCorrectTest()
    {
        var name1 = new Name("name", Substitute.For<IFrame>());
        var name2 = new Name("name", Substitute.For<IFrame>());
            
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
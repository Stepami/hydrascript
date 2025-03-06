using HydraScript.Domain.BackEnd.Impl.Addresses;

namespace HydraScript.UnitTests.Domain.BackEnd;

public class HashAddressTests
{
    [Fact]
    public void EqualsReturnsFalseForTwoDifferentObjectsWithSameSeed()
    {
        const int seed = 1;

        var addressOne = new HashAddress(seed);
        var addressTwo = new HashAddress(seed);

        Assert.NotEqual(addressOne, addressTwo);
    }
    
    [Fact]
    public void EqualsReturnsTrueForTwoSameObjectsWithSameSeed()
    {
        var address = new HashAddress(1);

        Assert.Equal(address, address);
    }
    
    [Fact]
    public void EqualsReturnsFalseForTwoObjectsWithDifferentSeed()
    {
        var addressOne = new HashAddress(0);
        var addressTwo = new HashAddress(1);

        Assert.NotEqual(addressOne, addressTwo);
    }
}
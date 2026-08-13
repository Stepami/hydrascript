using HydraScript.Domain.IR.Types;

namespace HydraScript.UnitTests.Domain.IR;

public class ObjectTypeTests
{
    [Fact]
    public void ObjectTypeEqualityTest()
    {
        var number = new Type("number");
        var point2Num1 = new ObjectType(new Dictionary<string, Type>(
        [
            new("x", number),
            new("y", number)
        ]));
        var point2Num2 = new ObjectType(new Dictionary<string, Type>(
        [
            new("x", number),
            new("y", number)
        ]));
        Assert.Equal(point2Num1, point2Num2);

        var point3Num1 = new ObjectType(new Dictionary<string, Type>(
        [
            new("a", number),
            new("x", number),
            new("y", number)
        ]));
        var point3Num2 = new ObjectType(new Dictionary<string, Type>(
        [
            new("y", number),
            new("x", number),
            new("z", number)
        ]));
        Assert.NotEqual(point3Num1, point3Num2);
        Assert.NotEqual(point3Num2, point2Num1);
    }
        
    [Fact]
    public void RecursiveTypeReferenceResolvingTest()
    {
        var number = new Type("number");
        var array = new ArrayType(new Type("self"));
        var nullable = new NullableType(new Type("self"));
        var linkedListType = new ObjectType(new Dictionary<string, Type>(
        [
            new("data", number),
            new("wrapped", new ObjectType(new Dictionary<string, Type>(
            [
                new("next", new Type("self"))
            ]))),
            new("children", array),
            new("parent", nullable)
        ]));

        linkedListType.ResolveReference(linkedListType, refId: "self");
            
        Assert.Equal(linkedListType, ((ObjectType)linkedListType["wrapped"]!)["next"]);
        Assert.Equal(linkedListType, array.Type);
        Assert.Equal(linkedListType, nullable.Type);
    }

    [Fact]
    public void NonSpecifiedTypesVisitingTest()
    {
        var objectType = new ObjectType(new Dictionary<string, Type>(
        [
            new("any", new Any()),
            new("some", new NullType()),
            new("next", new Type("self")),
            new("prop", new Type("number"))
        ]));
        var ex = Record.Exception(
            () => objectType.ResolveReference(
                objectType,
                refId: "self"));
        Assert.Null(ex);
        Assert.Equal(objectType["next"], objectType);
    }

    [Fact]
    public void ObjectTypeToStringTest()
    {
        var number = new Type("number");
        var array = new ArrayType(new Type("self"));
        var nullable = new NullableType(new Type("self"));
        var linkedListType = new ObjectType(new Dictionary<string, Type>(
        [
            new("data", number),
            new("wrapped", new ObjectType(new Dictionary<string, Type>(
            [
                new("next", new Type("self"))
            ]))),
            new("children", array),
            new("parent", nullable)
        ]));

        linkedListType.ResolveReference(linkedListType, refId: "self");

        Assert.Contains("@this", linkedListType.ToString());
    }

    [Fact]
    public void SerializationOfTypeWithRecursivePropertyTest()
    {
        var nodeType = new ObjectType(new Dictionary<string, Type>(
        [
            new("data", new Type("number")),
            new("next", new Type("self"))
        ]));
        nodeType.ResolveReference(nodeType, refId: "self");

        var linkedListType = new ObjectType(new Dictionary<string, Type>(
        [
            new("head", nodeType)
        ]));
        linkedListType.ResolveReference(linkedListType, refId: "self");

        Assert.Contains("next: next;", linkedListType.ToString());
    }

    [Theory, ClassData(typeof(IsSubsetOfData))]
    public void IsSubsetOf_SpecificSubset_ReturnsExpectedResult(
        ObjectType superset,
        ObjectType subset,
        bool expected)
    {
        // Act
        var result = superset.IsSubsetOf(subset);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory, ClassData(typeof(CalculateDifferenceData))]
    public void CalculateDifference_SpecificObjectType_ReturnsExpectedDifference(
        ObjectType superset,
        ObjectType subset,
        IReadOnlyList<string> expected)
    {
        // Act
        var result = superset.CalculateDifference(subset);

        // Assert
        Assert.Equal(expected.Count, result.Count);
        foreach (var propertyName in expected)
        {
            Assert.Contains(propertyName, result);
        }
    }
}
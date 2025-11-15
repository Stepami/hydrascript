using HydraScript.Domain.IR.Types;

namespace HydraScript.UnitTests.Domain.IR;

public class ObjectTypeTests
{
    [Fact]
    public void ObjectTypeEqualityTest()
    {
        var number = new Type("number");
        var p2d1 = new ObjectType(new Dictionary<string, Type>(
        [
            new("x", number),
            new("y", number)
        ]));
        var p2d2 = new ObjectType(new Dictionary<string, Type>(
        [
            new("x", number),
            new("y", number)
        ]));
        Assert.Equal(p2d1, p2d2);

        var p3d1 = new ObjectType(new Dictionary<string, Type>(
        [
            new("a", number),
            new("x", number),
            new("y", number)
        ]));
        var p3d2 = new ObjectType(new Dictionary<string, Type>(
        [
            new("y", number),
            new("x", number),
            new("z", number)
        ]));
        Assert.NotEqual(p3d1, p3d2);
        Assert.NotEqual(p3d2, p2d1);
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

    [Fact]
    public void IsSubsetOf_SubsetHasFewerProperties_ReturnsTrue()
    {
        // Arrange
        var number = new Type("number");
        var supersetType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number,
            ["z"] = number
        });
        var subsetType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number
        });

        // Act
        var result = supersetType.IsSubsetOf(subsetType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSubsetOf_SubsetHasSameProperties_ReturnsTrue()
    {
        // Arrange
        var number = new Type("number");
        var supersetType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number
        });
        var subsetType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number
        });

        // Act
        var result = supersetType.IsSubsetOf(subsetType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSubsetOf_SubsetHasExtraProperties_ReturnsFalse()
    {
        // Arrange
        var number = new Type("number");
        var supersetType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number
        });
        var subsetType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number,
            ["z"] = number
        });

        // Act
        var result = supersetType.IsSubsetOf(subsetType);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSubsetOf_PropertyTypesMismatch_ReturnsFalse()
    {
        // Arrange
        var number = new Type("number");
        var stringType = new Type("string");
        var supersetType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number
        });
        var subsetType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = stringType
        });

        // Act
        var result = supersetType.IsSubsetOf(subsetType);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSubsetOf_EmptySubset_ReturnsTrue()
    {
        // Arrange
        var number = new Type("number");
        var supersetType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number
        });
        var emptySubsetType = new ObjectType(new Dictionary<string, Type>());

        // Act
        var result = supersetType.IsSubsetOf(emptySubsetType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSubsetOf_SameObjectReference_ReturnsTrue()
    {
        // Arrange
        var number = new Type("number");
        var objectType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number
        });

        // Act
        var result = objectType.IsSubsetOf(objectType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSubsetOf_ComplexNestedTypes_ReturnsTrue()
    {
        // Arrange
        var number = new Type("number");
        var nestedObject = new ObjectType(new Dictionary<string, Type>
        {
            ["a"] = number,
            ["b"] = number
        });
        var arrayType = new ArrayType(number);

        var supersetType = new ObjectType(new Dictionary<string, Type>
        {
            ["nested"] = nestedObject,
            ["array"] = arrayType,
            ["simple"] = number
        });
        var subsetType = new ObjectType(new Dictionary<string, Type>
        {
            ["nested"] = nestedObject,
            ["array"] = arrayType
        });

        // Act
        var result = supersetType.IsSubsetOf(subsetType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSubsetOf_ComplexNestedTypesMismatch_ReturnsFalse()
    {
        // Arrange
        var number = new Type("number");
        var nestedObject1 = new ObjectType(new Dictionary<string, Type>
        {
            ["a"] = number,
            ["b"] = number
        });
        var nestedObject2 = new ObjectType(new Dictionary<string, Type>
        {
            ["a"] = number,
            ["c"] = number
        });

        var supersetType = new ObjectType(new Dictionary<string, Type>
        {
            ["nested"] = nestedObject1
        });
        var subsetType = new ObjectType(new Dictionary<string, Type>
        {
            ["nested"] = nestedObject2
        });

        // Act
        var result = supersetType.IsSubsetOf(subsetType);

        // Assert
        Assert.False(result);
    }
}
using Interpreter.Lib.IR.CheckSemantics.Types;
using Xunit;

namespace Interpreter.Tests.Unit.IR.Types;

public class ObjectTypeTests
{
    [Fact]
    public void ObjectTypeEqualityTest()
    {
        var number = new Type("number");
        var p2d1 = new ObjectType(
            new PropertyType[]
            {
                new("x", number), 
                new("y", number)
            }
        );
        var p2d2 = new ObjectType(
            new PropertyType[]
            {
                new("y", number), 
                new("x", number)
            }
        );
        Assert.Equal(p2d1, p2d2);

        var p3d1 = new ObjectType(
            new PropertyType[]
            {
                new("a", number),
                new("x", number),
                new("y", number)
            }
        );
        var p3d2 = new ObjectType(
            new PropertyType[]
            {
                new("y", number), 
                new("x", number),
                new("z", number)
            }
        );
        Assert.NotEqual(p3d1, p3d2);
        Assert.NotEqual(p3d2, p2d1);
    }
        
    [Fact]
    public void RecursiveTypeReferenceResolvingTest()
    {
        var number = new Type("number");
        var array = new ArrayType(new Type("self"));
        var method = new FunctionType(number, new List<Type> { new("self") });
        var nullable = new NullableType(new Type("self"));
        var linkedListType = new ObjectType(
            new List<PropertyType>
            {
                new("data", number),
                new("wrapped", new ObjectType(new List<PropertyType>
                {
                    new("next", new Type("self"))
                })),
                new("children", array),
                new("parent", nullable),
                new("compare", method)
            }
        );

        linkedListType.ResolveReference(linkedListType, refId: "self");
            
        Assert.Equal(linkedListType, ((ObjectType)linkedListType["wrapped"])["next"]);
        Assert.Equal(linkedListType, array.Type);
        Assert.Equal(linkedListType, nullable.Type);
        Assert.Equal(linkedListType, method.Arguments[0]);
    }

    [Fact]
    public void NonSpecifiedTypesVisitingTest()
    {
        var objectType = new ObjectType(
            new List<PropertyType>
            {
                new("any", new Any()),
                new("some", new NullType()),
                new("next", new Type("self")),
                new("prop", new Type("number"))
            }
        );
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
        var method = new FunctionType(number, new List<Type> { new("self") });
        var nullable = new NullableType(new Type("self"));
        var linkedListType = new ObjectType(
            new List<PropertyType>
            {
                new("data", number),
                new("wrapped", new ObjectType(new List<PropertyType>
                {
                    new("next", new Type("self"))
                })),
                new("children", array),
                new("parent", nullable),
                new("compare", method)
            }
        );

        linkedListType.ResolveReference(linkedListType, refId: "self");

        Assert.Contains("@this", linkedListType.ToString());
    }

    [Fact]
    public void SerializationOfTypeWithRecursivePropertyTest()
    {
        var nodeType = new ObjectType(
            new List<PropertyType>
            {
                new("data", new Type("number")),
                new("next", new Type("self"))
            }
        );
        nodeType.ResolveReference(nodeType, refId: "self");

        var linkedListType = new ObjectType(
            new List<PropertyType>
            {
                new("head", nodeType),
                new("append", new FunctionType(
                        new Type("void"),
                        new List<Type> { nodeType }
                    )
                ),
                new("copy", new FunctionType(new Type("self"), Array.Empty<Type>()))
            }
        );
        linkedListType.ResolveReference(linkedListType, refId: "self");

        Assert.Contains("head: head;", linkedListType.ToString());
    }
}
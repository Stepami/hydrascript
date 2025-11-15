using HydraScript.Domain.IR.Types;

namespace HydraScript.UnitTests.Domain.IR;

public sealed class IsSubsetOfData : TheoryData<ObjectType, ObjectType, bool>
{
    public IsSubsetOfData()
    {
        Type number = "number";
        Type stringType = "string";

        // Subset has fewer properties
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number,
                ["z"] = number
            }),
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number
            }),
            true
        );

        // Subset has same properties
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number
            }),
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number
            }),
            true
        );

        // Subset has extra properties
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number
            }),
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number,
                ["z"] = number
            }),
            false
        );

        // Property types mismatch
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number
            }),
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = stringType
            }),
            false
        );

        // Empty subset
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number
            }),
            new ObjectType(new Dictionary<string, Type>()),
            true
        );

        // Same object reference
        var objectType = new ObjectType(new Dictionary<string, Type>
        {
            ["x"] = number,
            ["y"] = number
        });
        Add(
            objectType,
            objectType,
            true
        );

        // Complex nested types
        var nestedObject = new ObjectType(new Dictionary<string, Type>
        {
            ["a"] = number,
            ["b"] = number
        });
        var arrayType = new ArrayType(number);
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["nested"] = nestedObject,
                ["array"] = arrayType,
                ["simple"] = number
            }),
            new ObjectType(new Dictionary<string, Type>
            {
                ["nested"] = nestedObject,
                ["array"] = arrayType
            }),
            true
        );

        // Complex nested types mismatch
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
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["nested"] = nestedObject1
            }),
            new ObjectType(new Dictionary<string, Type>
            {
                ["nested"] = nestedObject2
            }),
            false
        );
    }
}
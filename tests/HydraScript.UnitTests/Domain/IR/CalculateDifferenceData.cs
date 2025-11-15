using HydraScript.Domain.IR.Types;

namespace HydraScript.UnitTests.Domain.IR;

public sealed class CalculateDifferenceData :
    TheoryData<ObjectType, ObjectType, IReadOnlyList<string>>
{
    public CalculateDifferenceData()
    {
        Type number = "number";
        Type stringType = "string";

        // Current has extra properties
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
            new List<string> { "z" }
        );

        // Same properties
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
            new List<string>()
        );

        // Current is subset
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
            new List<string>()
        );

        // That is empty
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number,
                ["z"] = number
            }),
            new ObjectType(new Dictionary<string, Type>()),
            new List<string> { "x", "y", "z" }
        );

        // Current is empty
        Add(
            new ObjectType(new Dictionary<string, Type>()),
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number
            }),
            new List<string>()
        );

        // Multiple different properties
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["a"] = number,
                ["b"] = stringType,
                ["x"] = number,
                ["y"] = number,
                ["z"] = stringType
            }),
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number
            }),
            new List<string> { "a", "b", "z" }
        );

        // Completely different properties
        Add(
            new ObjectType(new Dictionary<string, Type>
            {
                ["a"] = number,
                ["b"] = number
            }),
            new ObjectType(new Dictionary<string, Type>
            {
                ["x"] = number,
                ["y"] = number
            }),
            new List<string> { "a", "b" }
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
            new List<string>()
        );
    }
}
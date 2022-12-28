using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Values;
using SystemType = System.Type;

namespace Interpreter.Lib.BackEnd.Instructions;

public class AsString : Simple
{
    public AsString(string left, IValue right) :
        base(left, (null, right), "") { }

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        frame[Left] = JsonSerializer.Serialize(
            right.right.Get(frame),
            new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                Converters = { new DoubleValueWriteConverter() },
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            }
        );

        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"{Left} = {right.right} as string";
        
    [ExcludeFromCodeCoverage]
    private class DoubleValueWriteConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader,
            SystemType typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, 
            double value, JsonSerializerOptions options)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (value == Math.Truncate(value))
                writer.WriteNumberValue(Convert.ToInt64(value));
            else
                writer.WriteNumberValue(value);
        }
    }
}
using System;
using Newtonsoft.Json;

namespace Interpreter.Lib
{
    public class DoubleValueConverter : JsonConverter<double>
    {
        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            writer.WriteRawValue(value == Math.Truncate(value)
                ? JsonConvert.ToString(Convert.ToInt64(value))
                : JsonConvert.ToString(value));
        }

        public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException("CanRead is false, so reading is unnecessary");
        }
    }
}
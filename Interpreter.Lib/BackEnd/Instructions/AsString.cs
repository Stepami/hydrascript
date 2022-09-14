using System;
using System.Collections.Generic;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.BackEnd.VM;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Interpreter.Lib.BackEnd.Instructions
{
    public class AsString : Simple
    {
        public AsString(string left, IValue right, int number) :
            base(left, (null, right), "", number)
        {
        }

        public override int Execute(VirtualMachine vm)
        {
            var frame = vm.Frames.Peek();
            frame[Left] = JsonConvert.SerializeObject(
                right.right.Get(frame),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    FloatFormatHandling = FloatFormatHandling.Symbol,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = new List<JsonConverter>
                    {
                        new DoubleValueConverter()
                    }
                }
            );

            return Jump();
        }

        protected override string ToStringRepresentation() => $"{Left} = {right.right} as string";
        
        private class DoubleValueConverter : JsonConverter<double>
        {
            public override bool CanRead => false;

            public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer) =>
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                writer.WriteRawValue(value == Math.Truncate(value)
                    ? JsonConvert.ToString(Convert.ToInt64(value))
                    : JsonConvert.ToString(value));

            public override double ReadJson(JsonReader reader, Type objectType,
                double existingValue, bool hasExistingValue,
                JsonSerializer serializer) =>
                throw new NotImplementedException("CanRead is false, so reading is unnecessary");
        }
    }
}
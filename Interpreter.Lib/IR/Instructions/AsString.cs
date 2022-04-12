using System.Collections.Generic;
using Interpreter.Lib.VM;
using Interpreter.Lib.VM.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Interpreter.Lib.IR.Instructions
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
    }
}
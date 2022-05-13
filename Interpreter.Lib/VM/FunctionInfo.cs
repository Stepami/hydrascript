namespace Interpreter.Lib.VM
{
    public class FunctionInfo
    {
        public string Id { get; }
        
        public int Location { get; set; }

        public string MethodOf { get; set; }

        public FunctionInfo(string id, int location = 0, string methodOf = null)
        {
            Id = id;
            Location = location;
            MethodOf = methodOf;
        }

        public string CallId() =>
            MethodOf == null
                ? Id
                : $"{MethodOf}.{Id}";

        public override string ToString() => $"({Location}, {CallId()})";
    }
}
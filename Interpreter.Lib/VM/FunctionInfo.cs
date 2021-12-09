namespace Interpreter.Lib.VM
{
    public class FunctionInfo
    {
        public string Id { get; }
        
        public int Location { get; set; }

        public FunctionInfo(string id, int location = 0)
        {
            Id = id;
            Location = location;
        }

        public override string ToString() => $"({Location}, {Id})";
    }
}
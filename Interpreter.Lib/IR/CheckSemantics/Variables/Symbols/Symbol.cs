namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols
{
    public abstract class Symbol
    {
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        public virtual string Id { get; }

        protected Symbol(string id)
        {
            Id = id;
        }
    }
}
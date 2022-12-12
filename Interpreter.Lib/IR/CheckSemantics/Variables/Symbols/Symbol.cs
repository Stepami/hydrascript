namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols
{
    public abstract class Symbol
    {
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        public virtual string Id { get; }
        
        public virtual Type Type { get; }

        protected Symbol(string id, Type type) =>
            (Id, Type) = (id, type);
    }
}
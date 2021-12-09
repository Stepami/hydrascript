namespace Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes
{
    internal record EndOfProgramType : TokenType
    {
        public EndOfProgramType() : base("EOP", "", int.MaxValue - 1)
        {
        }

        public override bool EndOfProgram()
        {
            return true;
        }

        public override string ToString()
        {
            return Tag;
        }
    }
}
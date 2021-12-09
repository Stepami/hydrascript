using AutoMapper;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Models;

namespace Interpreter.Services
{
    public class LexerCreatorService : ILexerCreatorService
    {
        private readonly IMapper mapper;

        public LexerCreatorService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Lexer CreateLexer(LexerQueryModel lexerQuery)
        {
            var domain = lexerQuery.GetDomain(mapper);
            var source = lexerQuery.Text;
            return new Lexer(domain, source);
        }
    }
}
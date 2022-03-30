using AutoMapper;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Models;

namespace Interpreter.Services
{
    public class LexerCreatorService : ILexerCreatorService
    {
        private readonly IMapper _mapper;

        public LexerCreatorService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Lexer CreateLexer(LexerQueryModel lexerQuery)
        {
            var domain = lexerQuery.GetDomain(_mapper);
            var source = lexerQuery.Text;
            return new Lexer(domain, source);
        }
    }
}
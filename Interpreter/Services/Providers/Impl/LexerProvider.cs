using AutoMapper;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Models;

namespace Interpreter.Services.Providers.Impl
{
    public class LexerProvider : ILexerProvider
    {
        private readonly IMapper _mapper;

        public LexerProvider(IMapper mapper)
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
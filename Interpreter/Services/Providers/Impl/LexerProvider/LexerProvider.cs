using AutoMapper;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Models;
using Microsoft.Extensions.Options;

namespace Interpreter.Services.Providers.Impl.LexerProvider
{
    public class LexerProvider : ILexerProvider
    {
        private readonly IMapper _mapper;
        private readonly CommandLineSettings _settings;

        public LexerProvider(IMapper mapper, IOptions<CommandLineSettings> options)
        {
            _mapper = mapper;
            _settings = options.Value;
        }

        public ILexer CreateLexer()
        {
            var domain = _mapper.Map<StructureModel, Structure>(_settings.StructureModel);
            var lexer = new Lexer(domain);
            return _settings.Dump
                ? new LoggingLexer(lexer, _settings.GetInputFileName())
                : lexer;
        }
    }
}
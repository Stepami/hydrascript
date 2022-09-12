using AutoMapper;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Models;
using Microsoft.Extensions.Options;

namespace Interpreter.Services.Providers.Impl
{
    public class LexerProvider : ILexerProvider
    {
        private readonly IMapper _mapper;
        private readonly StructureModel _structureModel;

        public LexerProvider(IMapper mapper, IOptions<CommandLineSettings> options)
        {
            _mapper = mapper;
            _structureModel = options.Value.StructureModel;
        }

        public ILexer CreateLexer()
        {
            var domain = _mapper.Map<StructureModel, Structure>(_structureModel);
            return new Lexer(domain);
        }
    }
}
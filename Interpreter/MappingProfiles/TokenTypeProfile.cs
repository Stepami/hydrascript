using AutoMapper;
using Interpreter.Lib.FrontEnd.GetTokens.Impl.TokenTypes;
using Interpreter.Models;

namespace Interpreter.MappingProfiles
{
    public class TokenTypeProfile : Profile
    {
        public TokenTypeProfile()
        {
            CreateMap<TokenTypeModel, TokenType>()
                .Include<TokenTypeModel, WhiteSpaceType>()
                .ConstructUsing((src, _) => src.WhiteSpace ? new WhiteSpaceType() : new TokenType());

            CreateMap<TokenTypeModel, WhiteSpaceType>();
        }
    }
}
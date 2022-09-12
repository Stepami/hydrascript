using System.Collections.Generic;
using AutoMapper;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.FrontEnd.GetTokens.Impl.TokenTypes;
using Interpreter.Models;

namespace Interpreter.MappingProfiles
{
    public class StructureProfile : Profile
    {
        public StructureProfile()
        {
            CreateMap<StructureModel, Structure>()
                .ConstructUsing((src, context) =>
                    new Structure(context.Mapper
                        .Map<List<TokenTypeModel>, List<TokenType>>
                            (src.TokenTypes)));
        }
    }
}
using System.Collections.Generic;
using AutoMapper;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.FrontEnd.TopDownParse;
using Interpreter.Lib.FrontEnd.TopDownParse.Impl;
using Interpreter.MappingProfiles;
using Interpreter.Models;
using Interpreter.Tests.TestData;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class ParserTests
    {
        private readonly IParser _parser;

        public ParserTests()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(
                x => x.AddProfiles(new List<Profile>
                {
                    new TokenTypeProfile(),
                    new StructureProfile()
                })
            ));

            _parser = new Parser(new Lexer(
                mapper.Map<StructureModel, Structure>(new())
            ));
        }

        [Theory]
        [ClassData(typeof(ParserSuccessTestData))]
        public void ParserDoesNotThrowTest(string text)
        {
            var ex = Record.Exception(() =>
            {
                // ReSharper disable once UnusedVariable
                var ast = _parser.TopDownParse(text);
            });
            Assert.Null(ex);
        }
    }
}
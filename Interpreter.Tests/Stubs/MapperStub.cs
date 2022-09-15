using System;
using System.Collections.Generic;
using AutoMapper;
using Interpreter.MappingProfiles;

namespace Interpreter.Tests.Stubs
{
    public class MapperStub : Mapper
    {
        public MapperStub() : base(new MapperConfiguration(
            x => x.AddProfiles(new List<Profile>
            {
                new TokenTypeProfile(),
                new StructureProfile()
            })
        )) { }
    }
}
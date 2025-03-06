using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.IR;
using HydraScript.Domain.IR.Impl.Symbols;
using PolymorphicContracts.AutoFixture;

namespace HydraScript.UnitTests;

public class AutoHydraScriptDataAttribute() :
    AutoDataAttribute(() =>
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

        fixture.Inject<ITextCoordinateSystemComputer>(new TextCoordinateSystemComputer());

        fixture.CustomizePolymorphismFor<ISymbol>()
            .WithDerivedType<VariableSymbol>()
            .WithDerivedType<TypeSymbol>()
            .WithDerivedType<ObjectSymbol>()
            .BuildCustomization();

        return fixture;
    });
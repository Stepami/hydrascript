using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit3;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.IR;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.UnitTests.Domain.BackEnd;
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

        fixture.Register<IFrameContext>(() => new FrameContext());
        fixture.Register<TestVirtualMachine, IExecuteParams>(vm => vm.ExecuteParams);

        return fixture;
    });
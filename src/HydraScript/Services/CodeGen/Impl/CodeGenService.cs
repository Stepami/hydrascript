using System.IO.Abstractions;
using HydraScript.Domain.BackEnd;
using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.Ast.Visitors.Services.Impl;
using HydraScript.Lib.IR.CheckSemantics.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;
using Microsoft.Extensions.Options;
using Visitor.NET;
using Type = HydraScript.Lib.IR.CheckSemantics.Types.Type;

namespace HydraScript.Services.CodeGen.Impl;

internal class CodeGenService : ICodeGenService
{
    private readonly IVisitor<IAbstractSyntaxTreeNode> _symbolTableInitializer;
    private readonly IVisitor<IAbstractSyntaxTreeNode> _typeSystemLoader;
    private readonly IVisitor<IAbstractSyntaxTreeNode> _declarationVisitor;

    private readonly IVisitor<IAbstractSyntaxTreeNode, Type> _semanticChecker;
    private readonly IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions> _instructionProvider;

    private readonly IFileSystem _fileSystem;
    private readonly CommandLineSettings _settings;

    public CodeGenService(IFileSystem fileSystem, IOptions<CommandLineSettings> options)
    {
        _fileSystem = fileSystem;
        _settings = options.Value;

        var functionStorage = new FunctionWithUndefinedReturnStorage();
        var methodStorage = new MethodStorage();
        var symbolTables = new SymbolTableStorage();
        
        _symbolTableInitializer = new SymbolTableInitializer(
            new StandardLibraryProvider(
                new JavaScriptTypesProvider()),
            symbolTables);
        _typeSystemLoader = new TypeSystemLoader(
            new TypeDeclarationsResolver(
                new JavaScriptTypesProvider(),
                symbolTables),
            new JavaScriptTypesProvider(),
            symbolTables);
        _declarationVisitor = new DeclarationVisitor(functionStorage, methodStorage, symbolTables);
        
        _semanticChecker = new SemanticChecker(
            new DefaultValueForTypeCalculator(),
            functionStorage,
            methodStorage,
            symbolTables);
        _instructionProvider = new InstructionProvider(new ValueDtoConverter());
    }

    public AddressedInstructions GetInstructions(IAbstractSyntaxTree ast)
    {
        var root = ast.Root;
        root.Accept(_symbolTableInitializer);
        root.Accept(_typeSystemLoader);
        root.Accept(_declarationVisitor);

        root.Accept(_semanticChecker);
        var instructions = root.Accept(_instructionProvider);

        if (_settings.Dump)
        {
            var fileName = _settings.InputFilePath.Split(".js")[0];
            _fileSystem.File.WriteAllLines(
                $"{fileName}.tac",
                instructions.Select(i => i.ToString()!));
        }

        return instructions;
    }
}
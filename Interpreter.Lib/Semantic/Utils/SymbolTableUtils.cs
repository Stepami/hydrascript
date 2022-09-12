using System.Collections.Generic;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.Semantic.Nodes;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Nodes.Statements;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Utils
{
    public static class SymbolTableUtils
    {
        public static SymbolTable GetStandardLibrary()
        {
            var library = new SymbolTable();
            
            library.AddType(TypeUtils.JavaScriptTypes.Number);
            library.AddType(TypeUtils.JavaScriptTypes.Boolean);
            library.AddType(TypeUtils.JavaScriptTypes.String);
            library.AddType(TypeUtils.JavaScriptTypes.Null);
            library.AddType(TypeUtils.JavaScriptTypes.Void);

            var print = new FunctionSymbol(
                "print",
                new List<Symbol>
                {
                    new VariableSymbol("str")
                    {
                        Type = TypeUtils.JavaScriptTypes.String
                    }
                },
                new FunctionType(TypeUtils.JavaScriptTypes.Void, new[] {TypeUtils.JavaScriptTypes.String})
            );
            print.Body = new FunctionDeclaration(
                print,
                new BlockStatement(new List<StatementListItem>())
                {
                    SymbolTable = new SymbolTable()
                }
            )
            {
                SymbolTable = new SymbolTable(),
                Segment = new Segment(
                    new Coordinates(0, 0),
                    new Coordinates(0, 0)
                )
            };

            library.AddSymbol(print);

            var symbolTable = new SymbolTable();
            symbolTable.AddOpenScope(library);
            return symbolTable;
        }
    }
}
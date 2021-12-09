using System.Collections.Generic;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.Semantic.Nodes;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Nodes.Statements;
using Interpreter.Lib.Semantic.Symbols;

namespace Interpreter.Lib.Semantic.Utils
{
    public static class SymbolTableUtils
    {
        public static SymbolTable GetStandardLibrary()
        {
            var library = new SymbolTable();

            var print = new FunctionSymbol(
                "print",
                new List<Symbol>
                {
                    new VariableSymbol("str")
                    {
                        Type = TypeUtils.JavaScriptTypes.String
                    }
                }
            )
            {
                ReturnType = TypeUtils.JavaScriptTypes.Void
            };
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

            var toString = new FunctionSymbol(
                "toString",
                new List<Symbol>
                {
                    new VariableSymbol("obj")
                    {
                        Type = TypeUtils.JavaScriptTypes.Object
                    }
                }
            )
            {
                ReturnType = TypeUtils.JavaScriptTypes.String
            };
            toString.Body = new FunctionDeclaration(
                toString,
                new BlockStatement(new List<StatementListItem>
                {
                    new ReturnStatement(
                        new Literal(TypeUtils.JavaScriptTypes.String, "")
                    )
                })
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
            library.AddSymbol(toString);

            var symbolTable = new SymbolTable();
            symbolTable.AddOpenScope(library);
            return symbolTable;
        }
    }
}
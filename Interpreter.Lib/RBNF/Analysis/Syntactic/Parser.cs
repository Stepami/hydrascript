using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Interpreter.Lib.RBNF.Analysis.Exceptions;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.Semantic;
using Interpreter.Lib.Semantic.Nodes;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Nodes.Expressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Nodes.Statements;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Utils;
using Expression = Interpreter.Lib.Semantic.Nodes.Expressions.Expression;

namespace Interpreter.Lib.RBNF.Analysis.Syntactic
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class Parser
    {
        private readonly IEnumerator<Token> tokens;
        private readonly Domain domain;

        public Parser(IEnumerable<Token> lexer, Domain domain)
        {
            tokens = lexer.GetEnumerator();
            tokens.MoveNext();
            this.domain = domain;
        }

        private Token Expect(string expectedTag, string expectedValue = null)
        {
            var current = tokens.Current;

            if (!CurrentIs(expectedTag))
                throw new ParserException(
                    $"{tokens.Current.Segment} expected = {expectedTag}; actual = {tokens.Current.Type.Tag}"
                );
            if (tokens.Current.Value != (expectedValue ?? tokens.Current.Value))
                throw new ParserException(
                    $"{tokens.Current.Segment} expected = {expectedValue}; actual = {tokens.Current.Value}"
                );

            if (CurrentIs(expectedTag) && tokens.Current.Value == (expectedValue ?? tokens.Current.Value))
            {
                tokens.MoveNext();
            }

            return current;
        }

        private bool CurrentIs(string tag) => tokens.Current.Type == domain.FindByTag(tag);

        private bool CurrentIsLiteral() => CurrentIs("NullLiteral") ||
                                           CurrentIs("IntegerLiteral") ||
                                           CurrentIs("FloatLiteral") ||
                                           CurrentIs("StringLiteral") ||
                                           CurrentIs("BooleanLiteral");

        private bool CurrentIsKeyword(string keyword) => CurrentIs("Keyword") && tokens.Current.Value == keyword;

        private bool CurrentIsOperator(string @operator) => CurrentIs("Operator") && tokens.Current.Value == @operator;

        private bool NextIs(string expectedTag)
        {
            var currentValue = tokens.Current;
            tokens.MoveNext();
            var result = CurrentIs(expectedTag);
            tokens.Reset();
            while (!currentValue.Equals(tokens.Current))
            {
                tokens.MoveNext();
            }

            return result;
        }

        public AbstractSyntaxTree TopDownParse()
        {
            var root = Script(SymbolTableUtils.GetStandardLibrary());
            Expect("EOP");
            return new AbstractSyntaxTree(root);
        }

        private ScriptBody Script(SymbolTable table = null) =>
            new(StatementList(table ?? new SymbolTable()))
            {
                SymbolTable = table ?? new SymbolTable()
            };

        private IEnumerable<StatementListItem> StatementList(SymbolTable table)
        {
            var statementList = new List<StatementListItem>();
            while (
                CurrentIsKeyword("function") || CurrentIsKeyword("let") || CurrentIsKeyword("const") ||
                CurrentIs("Ident") || CurrentIsLiteral() || CurrentIs("LeftParen") ||
                CurrentIs("LeftCurl") || CurrentIsKeyword("return") || CurrentIsKeyword("break") ||
                CurrentIsKeyword("continue") || CurrentIsKeyword("if") || CurrentIsKeyword("while")
            )
            {
                statementList.Add(StatementListItem(table));
            }

            return statementList;
        }

        private StatementListItem StatementListItem(SymbolTable table)
        {
            if (CurrentIsKeyword("function") || CurrentIsKeyword("let") || CurrentIsKeyword("const"))
            {
                return Declaration(table);
            }

            return Statement(table);
        }

        private Statement Statement(SymbolTable table)
        {
            if (CurrentIs("Ident") || CurrentIsLiteral() || CurrentIs("LeftParen") || CurrentIsOperator("-") ||
                CurrentIsOperator("!"))
            {
                return ExpressionStatement(table);
            }

            if (CurrentIs("LeftCurl"))
            {
                return BlockStatement(table);
            }

            if (CurrentIsKeyword("return"))
            {
                return ReturnStatement(table);
            }

            if (CurrentIsKeyword("break"))
            {
                return new BreakStatement
                {
                    Segment = Expect("Keyword", "break").Segment
                };
            }

            if (CurrentIsKeyword("continue"))
            {
                return new ContinueStatement
                {
                    Segment = Expect("Keyword", "continue").Segment
                };
            }

            if (CurrentIsKeyword("if"))
            {
                return IfStatement(table);
            }

            if (CurrentIsKeyword("while"))
            {
                return WhileStatement(table);
            }

            return null;
        }

        private BlockStatement BlockStatement(SymbolTable table)
        {
            var newTable = new SymbolTable();
            newTable.AddOpenScope(table);

            Expect("LeftCurl");
            var block = new BlockStatement(StatementList(newTable))
            {
                SymbolTable = newTable
            };
            Expect("RightCurl");

            return block;
        }

        private ExpressionStatement ExpressionStatement(SymbolTable table)
        {
            return new(Expression(table));
        }

        private ReturnStatement ReturnStatement(SymbolTable table)
        {
            var ret = Expect("Keyword", "return");
            if (CurrentIs("Ident") || CurrentIsLiteral() || CurrentIs("LeftParen") || CurrentIsOperator("-") ||
                CurrentIsOperator("!"))
            {
                return new ReturnStatement(Expression(table))
                {
                    Segment = ret.Segment,
                    SymbolTable = table
                };
            }

            return new ReturnStatement
            {
                Segment = ret.Segment
            };
        }

        private IfStatement IfStatement(SymbolTable table)
        {
            var token = Expect("Keyword", "if");
            Expect("LeftParen");
            var expr = Expression(table);
            Expect("RightParen");
            var then = Statement(table);
            if (CurrentIsKeyword("else"))
            {
                Expect("Keyword", "else");
                var @else = Statement(table);
                return new IfStatement(expr, then, @else) {SymbolTable = table, Segment = token.Segment};
            }

            return new IfStatement(expr, then) {SymbolTable = table, Segment = token.Segment};
        }

        private WhileStatement WhileStatement(SymbolTable table)
        {
            var token = Expect("Keyword", "while");
            Expect("LeftParen");
            var expr = Expression(table);
            Expect("RightParen");
            var stmt = Statement(table);
            return new WhileStatement(expr, stmt) {SymbolTable = table, Segment = token.Segment};
        }

        private Declaration Declaration(SymbolTable table)
        {
            if (CurrentIsKeyword("function"))
            {
                return FunctionDeclaration(table);
            }

            if (CurrentIsKeyword("let") || CurrentIsKeyword("const"))
            {
                return LexicalDeclaration(table);
            }

            return null;
        }

        private FunctionDeclaration FunctionDeclaration(SymbolTable table)
        {
            var newTable = new SymbolTable();
            newTable.AddOpenScope(table);

            Expect("Keyword", "function");
            var ident = Expect("Ident");

            Expect("LeftParen");
            var args = new List<Symbol>();
            if (CurrentIs("Ident"))
            {
                var arg = Expect("Ident").Value;
                Expect("Colon");
                var type = TypeUtils.GetJavaScriptType(Expect("Keyword").Value);
                args.Add(new VariableSymbol(arg)
                {
                    Type = type
                });
            }

            while (CurrentIs("Comma"))
            {
                Expect("Comma");
                var arg = Expect("Ident").Value;
                Expect("Colon");
                var type = TypeUtils.GetJavaScriptType(Expect("Keyword").Value);
                args.Add(new VariableSymbol(arg)
                {
                    Type = type
                });
            }

            Expect("RightParen");

            var returnType = TypeUtils.JavaScriptTypes.Void;
            if (CurrentIs("Colon"))
            {
                Expect("Colon");
                returnType = TypeUtils.GetJavaScriptType(Expect("Keyword").Value);
            }

            var functionSymbol = new FunctionSymbol(ident.Value, args) {ReturnType = returnType};
            table.AddSymbol(functionSymbol);

            return new FunctionDeclaration(functionSymbol, BlockStatement(newTable))
            {
                Segment = ident.Segment,
                SymbolTable = newTable
            };
        }

        private LexicalDeclaration LexicalDeclaration(SymbolTable table)
        {
            var readOnly = CurrentIsKeyword("const");
            Expect("Keyword", readOnly ? "const" : "let");
            var declaration = new LexicalDeclaration(readOnly)
            {
                SymbolTable = table
            };

            var ident = Expect("Ident");
            if (CurrentIs("Assign"))
            {
                Expect("Assign");
                declaration.AddAssignment(ident.Value, ident.Segment, Expression(table));
            }
            else if (CurrentIs("Colon"))
            {
                Expect("Colon");
                var type = TypeUtils.GetJavaScriptType(Expect("Keyword").Value);
                declaration.AddAssignment(
                    ident.Value,
                    ident.Segment,
                    new Literal(
                        type,
                        TypeUtils.GetDefaultValue(type)
                    )
                );
            }

            while (CurrentIs("Comma"))
            {
                Expect("Comma");
                ident = Expect("Ident");
                if (CurrentIs("Assign"))
                {
                    Expect("Assign");
                    declaration.AddAssignment(ident.Value, ident.Segment, Expression(table));
                }
                else if (CurrentIs("Colon"))
                {
                    Expect("Colon");
                    var type = TypeUtils.GetJavaScriptType(Expect("Keyword").Value);
                    declaration.AddAssignment(
                        ident.Value,
                        ident.Segment,
                        new Literal(
                            type,
                            TypeUtils.GetDefaultValue(type)
                        )
                    );
                }
            }

            return declaration;
        }

        private Expression Expression(SymbolTable table)
        {
            return AssignmentExpression(table);
        }

        private Expression AssignmentExpression(SymbolTable table)
        {
            if (CurrentIs("Ident") && NextIs("Assign"))
            {
                var ident = Expect("Ident");
                var id = new IdentifierReference(ident.Value) {SymbolTable = table, Segment = ident.Segment};
                if (CurrentIs("Assign"))
                {
                    var assign = Expect("Assign");
                    return new AssignmentExpression(id, AssignmentExpression(table))
                        {SymbolTable = table, Segment = assign.Segment};
                }
            }

            return ConditionalExpression(table);
        }

        private Expression ConditionalExpression(SymbolTable table)
        {
            var test = OrExpression(table);
            if (CurrentIs("QuestionMark"))
            {
                Expect("QuestionMark");
                var consequent = ConditionalExpression(table);
                Expect("Colon");
                var alternate = ConditionalExpression(table);
                return new ConditionalExpression(test, consequent, alternate);
            }

            return test;
        }

        private Expression OrExpression(SymbolTable table)
        {
            var left = AndExpression(table);
            while (CurrentIsOperator("||"))
            {
                var op = Expect("Operator");
                var right = AndExpression(table);
                left = new BinaryExpression(left, op.Value, right)
                {
                    Segment = op.Segment
                };
            }

            return left;
        }

        private Expression AndExpression(SymbolTable table)
        {
            var left = EqualityExpression(table);
            while (CurrentIsOperator("&&"))
            {
                var op = Expect("Operator");
                var right = EqualityExpression(table);
                left = new BinaryExpression(left, op.Value, right)
                {
                    Segment = op.Segment
                };
            }

            return left;
        }

        private Expression EqualityExpression(SymbolTable table)
        {
            var left = RelationExpression(table);
            while (CurrentIsOperator("==") || CurrentIsOperator("!="))
            {
                var op = Expect("Operator");
                var right = RelationExpression(table);
                left = new BinaryExpression(left, op.Value, right)
                {
                    Segment = op.Segment
                };
            }

            return left;
        }

        private Expression RelationExpression(SymbolTable table)
        {
            var left = AdditiveExpression(table);
            while (CurrentIsOperator(">") || CurrentIsOperator("<") || CurrentIsOperator(">=") ||
                   CurrentIsOperator("<="))
            {
                var op = Expect("Operator");
                var right = AdditiveExpression(table);
                left = new BinaryExpression(left, op.Value, right)
                {
                    Segment = op.Segment
                };
            }

            return left;
        }

        private Expression AdditiveExpression(SymbolTable table)
        {
            var left = MultiplicativeExpression(table);
            while (CurrentIsOperator("+") || CurrentIsOperator("-"))
            {
                var op = Expect("Operator");
                var right = MultiplicativeExpression(table);
                left = new BinaryExpression(left, op.Value, right)
                {
                    Segment = op.Segment
                };
            }

            return left;
        }

        private Expression MultiplicativeExpression(SymbolTable table)
        {
            var left = UnaryExpression(table);
            while (CurrentIsOperator("*") || CurrentIsOperator("/") || CurrentIsOperator("%"))
            {
                var op = Expect("Operator");
                var right = UnaryExpression(table);
                left = new BinaryExpression(left, op.Value, right)
                {
                    Segment = op.Segment
                };
            }

            return left;
        }

        private Expression UnaryExpression(SymbolTable table)
        {
            if (CurrentIsOperator("-") || CurrentIsOperator("!"))
            {
                var op = Expect("Operator");
                return new UnaryExpression(op.Value, UnaryExpression(table))
                {
                    Segment = op.Segment
                };
            }

            return PrimaryExpression(table);
        }

        private Expression PrimaryExpression(SymbolTable table)
        {
            if (CurrentIs("LeftParen"))
            {
                Expect("LeftParen");
                var expr = Expression(table);
                Expect("RightParen");
                return expr;
            }

            if (CurrentIs("Ident"))
            {
                var ident = Expect("Ident");
                var id = new IdentifierReference(ident.Value)
                {
                    Segment = ident.Segment,
                    SymbolTable = table
                };
                if (CurrentIs("LeftParen"))
                {
                    Expect("LeftParen");
                    var expressions = new List<Expression>();
                    if (CurrentIs("Ident") || CurrentIsLiteral() || CurrentIs("LeftParen") || CurrentIsOperator("-"))
                    {
                        expressions.Add(Expression(table));
                    }

                    while (CurrentIs("Comma"))
                    {
                        Expect("Comma");
                        expressions.Add(Expression(table));
                    }

                    Expect("RightParen");
                    return new CallExpression(id, expressions)
                    {
                        SymbolTable = table
                    };
                }

                return id;
            }

            if (CurrentIsLiteral())
            {
                return Literal();
            }

            return null;
        }

        private Literal Literal()
        {
            var segment = tokens.Current.Segment;
            if (CurrentIs("StringLiteral"))
            {
                var str = Expect("StringLiteral");
                return new Literal(
                    TypeUtils.JavaScriptTypes.String,
                    Regex.Unescape(str.Value.Trim('"')),
                    segment,
                    str.Value
                        .Replace(@"\", @"\\")
                        .Replace(@"""", @"\""")
                );
            }

            return tokens.Current.Type.Tag switch
            {
                "NullLiteral" => new Literal(TypeUtils.JavaScriptTypes.Null,
                    Expect("NullLiteral").Value == "null" ? null : "", segment),
                "IntegerLiteral" => new Literal(TypeUtils.JavaScriptTypes.Number,
                    double.Parse(Expect("IntegerLiteral").Value), segment),
                "FloatLiteral" => new Literal(TypeUtils.JavaScriptTypes.Number,
                    double.Parse(Expect("FloatLiteral").Value), segment),
                "BooleanLiteral" => new Literal(TypeUtils.JavaScriptTypes.Boolean,
                    bool.Parse(Expect("BooleanLiteral").Value), segment),
                _ => new Literal(TypeUtils.JavaScriptTypes.Undefined, new TypeUtils.Undefined())
            };
        }
    }
}
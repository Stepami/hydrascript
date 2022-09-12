using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.Semantic;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Nodes.Expressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Nodes.Statements;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
using Expression = Interpreter.Lib.Semantic.Nodes.Expressions.Expression;

namespace Interpreter.Lib.FrontEnd.TopDownParse.Impl
{
    public class Parser : IParser
    {
        private TokensStream _tokens;
        private readonly ILexer _lexer;

        public Parser(ILexer lexer) => 
            _lexer = lexer;

        private Token Expect(string expectedTag, string expectedValue = null)
        {
            var current = _tokens.Current;

            if (!CurrentIs(expectedTag))
                throw new ParserException(_tokens.Current!.Segment, expectedTag, _tokens.Current);
            if (_tokens.Current!.Value != (expectedValue ?? _tokens.Current.Value))
                throw new ParserException(_tokens.Current.Segment, expectedValue, _tokens.Current);

            if (CurrentIs(expectedTag) && _tokens.Current.Value == (expectedValue ?? _tokens.Current.Value))
            {
                _tokens.MoveNext();
            }

            return current;
        }

        private bool CurrentIs(string tag) =>
            _tokens.Current!.Type == _lexer.Structure.FindByTag(tag);

        private bool CurrentIsLiteral() =>
            CurrentIs("NullLiteral") ||
            CurrentIs("IntegerLiteral") ||
            CurrentIs("FloatLiteral") ||
            CurrentIs("StringLiteral") ||
            CurrentIs("BooleanLiteral");

        private bool CurrentIsKeyword(string keyword) =>
            CurrentIs("Keyword") &&
            _tokens.Current!.Value == keyword;

        private bool CurrentIsOperator(string @operator) =>
            CurrentIs("Operator") &&
            _tokens.Current!.Value == @operator;

        public AbstractSyntaxTree TopDownParse(string text)
        {
            _tokens = _lexer.GetTokens(text);
            
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
                CurrentIsKeyword("continue") || CurrentIsKeyword("if") || CurrentIsKeyword("while") ||
                CurrentIsKeyword("type")
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

            if (CurrentIsKeyword("type"))
            {
                return TypeStatement(table);
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
                CurrentIsOperator("!") || CurrentIs("LeftCurl") || CurrentIs("LeftBracket"))
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

        private TypeStatement TypeStatement(SymbolTable table)
        {
            var typeWord = Expect("Keyword", "type");
            var ident = Expect("Ident");
            Expect("Assign");
            if (CurrentIs("LeftCurl"))
            {
                table.AddType(new Type(ident.Value));
            }
            var type = TypeValue(table);

            if (type is ObjectType objectType)
            {
                objectType.ResolveSelfReferences(ident.Value);
            }
            table.AddType(type, ident.Value);
            
            return new TypeStatement(ident.Value, type)
            {
                Segment = typeWord.Segment,
                SymbolTable = table
            };
        }

        private Type TypeValue(SymbolTable table)
        {
            if (CurrentIs("Ident"))
            {
                var ident = Expect("Ident");
                var typeFromTable = table.FindType(ident.Value);
                if (typeFromTable == null)
                {
                    throw new UnknownIdentifierReference(
                        new IdentifierReference(ident.Value)
                            {Segment = ident.Segment}
                    );
                }

                return WithSuffix(typeFromTable);
            }

            if (CurrentIs("LeftCurl"))
            {
                Expect("LeftCurl");
                var propertyTypes = new List<PropertyType>();
                while (CurrentIs("Ident"))
                {
                    var ident = Expect("Ident");
                    Expect("Colon");
                    var propType = TypeValue(table); 
                    propertyTypes.Add(new PropertyType(ident.Value, propType));
                    Expect("SemiColon");
                }

                Expect("RightCurl");
                
                return WithSuffix(new ObjectType(propertyTypes));
            }

            if (CurrentIs("LeftParen"))
            {
                Expect("LeftParen");
                var args = new List<Type>();
                while (CurrentIs("Ident") || CurrentIs("LeftCurl") || CurrentIs("LeftParen"))
                {
                    args.Add(TypeValue(table));
                    if (!CurrentIs("RightParen"))
                    {
                        Expect("Comma");
                    }
                }
                Expect("RightParen");
                Expect("Arrow");
                var returnType = TypeValue(table);
                return new FunctionType(returnType, args);
            }

            return null;
        }

        private Type WithSuffix(Type baseType)
        {
            var type = baseType;
            while (CurrentIs("LeftBracket") || CurrentIs("QuestionMark"))
            {
                if (CurrentIs("LeftBracket"))
                {
                    Expect("LeftBracket");
                    Expect("RightBracket");
                    type = new ArrayType(type);
                } 
                else if (CurrentIs("QuestionMark"))
                {
                    Expect("QuestionMark");
                    type = new NullableType(type);
                }
            }

            return type;
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
            var args = new List<VariableSymbol>();
            if (CurrentIs("Ident"))
            {
                var arg = Expect("Ident").Value;
                Expect("Colon");
                var type = TypeValue(table);
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
                var type = TypeValue(table);
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
                returnType = TypeValue(table);
            }

            var functionSymbol =
                new FunctionSymbol(ident.Value, args,
                    new FunctionType(returnType, args.Select(x => x.Type))
                );
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

            AddToDeclaration(declaration, table);

            while (CurrentIs("Comma"))
            {
                Expect("Comma");
                AddToDeclaration(declaration, table);
            }

            return declaration;
        }

        private void AddToDeclaration(LexicalDeclaration declaration, SymbolTable table)
        {
            var ident = Expect("Ident");
            if (CurrentIs("Assign"))
            {
                var assignSegment = Expect("Assign").Segment;
                declaration.AddAssignment(ident.Value, ident.Segment, Expression(table), assignSegment);
            }
            else if (CurrentIs("Colon"))
            {
                Expect("Colon");
                var type = TypeValue(table);
                if (CurrentIs("Assign"))
                {
                    var assignSegment = Expect("Assign").Segment;
                    declaration.AddAssignment(ident.Value, ident.Segment, Expression(table), assignSegment, type);
                }
                else
                {
                    declaration.AddAssignment(
                        ident.Value,
                        ident.Segment,
                        new Literal(
                            type,
                            TypeUtils.GetDefaultValue(type),
                            label: TypeUtils.GetDefaultValue(type) == null ? "null" : null
                        )
                    );
                }
            }
        }

        private Expression Expression(SymbolTable table)
        {
            return AssignmentExpression(table);
        }

        private Expression AssignmentExpression(SymbolTable table)
        {
            var lhs = LeftHandSideExpression(table);
            if (CurrentIs("Assign") && !(lhs is CallExpression))
            {
                var assign = Expect("Assign");
                var member = lhs is IdentifierReference reference
                    ? (MemberExpression) reference
                    : (MemberExpression) lhs;
                return new AssignmentExpression(member, AssignmentExpression(table))
                    {SymbolTable = table, Segment = assign.Segment};
            }

            return lhs;
        }

        private Expression LeftHandSideExpression(SymbolTable table)
        {
            var expr = CastExpression(table);
            if (expr is IdentifierReference identRef)
            {
                if (CurrentIs("LeftParen") || CurrentIs("LeftBracket") || CurrentIs("Dot"))
                {
                    return CallExpression(identRef, table);
                }
            }

            return expr;
        }

        private Expression CallExpression(IdentifierReference identRef, SymbolTable table)
        {
            var member = MemberExpression(identRef, table);
            if (CurrentIs("LeftParen"))
            {
                var lp = Expect("LeftParen");
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
                return new CallExpression(member, expressions)
                {
                    SymbolTable = table,
                    Segment = lp.Segment
                };
            }

            return member;
        }

        private MemberExpression MemberExpression(IdentifierReference identRef, SymbolTable table)
        {
            var accessChain = new List<AccessExpression>();
            while (CurrentIs("LeftBracket") || CurrentIs("Dot"))
            {
                Token access;
                if (CurrentIs("LeftBracket"))
                {
                    access = Expect("LeftBracket");
                    var lb = access.Segment;
                    var expr = Expression(table);
                    var rb = Expect("RightBracket").Segment;
                    accessChain.Add(
                        new IndexAccess(expr, accessChain.LastOrDefault()) {Segment = lb + rb}
                    );
                }
                else if (CurrentIs("Dot"))
                {
                    access = Expect("Dot");
                    var identToken = Expect("Ident");
                    var idRef = new IdentifierReference(identToken.Value)
                    {
                        Segment = identToken.Segment,
                        SymbolTable = table
                    };
                    accessChain.Add(
                        new DotAccess(idRef, accessChain.LastOrDefault()) {Segment = access.Segment}
                    );
                }
            }
 
            return new MemberExpression(identRef, accessChain.FirstOrDefault())
            {
                SymbolTable = table
            };
        }

        private Expression CastExpression(SymbolTable table)
        {
            var cond = ConditionalExpression(table);
            if (CurrentIsKeyword("as"))
            {
                var asKeyword = Expect("Keyword", "as");
                var type = TypeValue(table);
                return new CastAsExpression(cond, type) {Segment = asKeyword.Segment};
            }

            return cond;
        }

        private Expression ConditionalExpression(SymbolTable table)
        {
            var test = OrExpression(table);
            if (CurrentIs("QuestionMark"))
            {
                Expect("QuestionMark");
                var consequent = AssignmentExpression(table);
                Expect("Colon");
                var alternate = AssignmentExpression(table);
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
            while (CurrentIsOperator("*") || CurrentIsOperator("/") || CurrentIsOperator("%")
                   || CurrentIsOperator("++") || CurrentIsOperator("::"))
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
            if (CurrentIsOperator("-") || CurrentIsOperator("!") || CurrentIsOperator("~"))
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

                return id;
            }

            if (CurrentIsLiteral())
            {
                return Literal();
            }

            if (CurrentIs("LeftCurl"))
            {
                return ObjectLiteral(table);
            }
            
            if (CurrentIs("LeftBracket"))
            {
                return ArrayLiteral(table);
            }

            return null;
        }

        private Literal Literal()
        {
            var segment = _tokens.Current!.Segment;
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

            return _tokens.Current.Type.Tag switch
            {
                "NullLiteral" => new Literal(TypeUtils.JavaScriptTypes.Null,
                    Expect("NullLiteral").Value == "null" ? null : "", segment, "null"),
                "IntegerLiteral" => new Literal(TypeUtils.JavaScriptTypes.Number,
                    double.Parse(Expect("IntegerLiteral").Value), segment),
                "FloatLiteral" => new Literal(TypeUtils.JavaScriptTypes.Number,
                    double.Parse(Expect("FloatLiteral").Value), segment),
                "BooleanLiteral" => new Literal(TypeUtils.JavaScriptTypes.Boolean,
                    bool.Parse(Expect("BooleanLiteral").Value), segment),
                _ => new Literal(TypeUtils.JavaScriptTypes.Undefined, new TypeUtils.Undefined())
            };
        }

        private ObjectLiteral ObjectLiteral(SymbolTable table)
        {
            var newTable = new SymbolTable();
            newTable.AddOpenScope(table);
            Expect("LeftCurl");
            var properties = new List<Property>();
            var methods = new List<FunctionDeclaration>();
            while (CurrentIs("Ident"))
            {
                var idToken = Expect("Ident");
                var id = new IdentifierReference(idToken.Value)
                {
                    Segment = idToken.Segment,
                    SymbolTable = newTable
                };
                if (CurrentIs("Colon"))
                {
                    Expect("Colon");
                    var expr = Expression(newTable);
                    properties.Add(new Property(id, expr));
                }
                else if (CurrentIs("Arrow"))
                {
                    Expect("Arrow");
                    Expect("LeftParen");
                    var args = new List<VariableSymbol>();
                    while (CurrentIs("Ident"))
                    {
                        var name = Expect("Ident").Value;
                        Expect("Colon");
                        var type = TypeValue(newTable);
                        args.Add(new VariableSymbol(name) {Type = type});
                        if (!CurrentIs("RightParen"))
                        {
                            Expect("Comma");
                        }
                    }
                    Expect("RightParen");
                    var returnType = TypeUtils.JavaScriptTypes.Void;
                    if (CurrentIs("Colon"))
                    {
                        Expect("Colon");
                        returnType = TypeValue(newTable);
                    }

                    var functionSymbol = new FunctionSymbol(idToken.Value, args,
                        new FunctionType(returnType, args.Select(a => a.Type))
                    );
                    newTable.AddSymbol(functionSymbol);
                    var bodyTable = new SymbolTable();
                    bodyTable.AddOpenScope(newTable);
                    methods.Add(new FunctionDeclaration(functionSymbol, BlockStatement(bodyTable))
                    {
                        Segment = idToken.Segment,
                        SymbolTable = bodyTable
                    });
                }

                Expect("SemiColon");
            }
            Expect("RightCurl");
            return new ObjectLiteral(properties, methods)
            {
                SymbolTable = newTable
            };
        }

        private ArrayLiteral ArrayLiteral(SymbolTable table)
        {
            var lb = Expect("LeftBracket").Segment;
            var expressions = new List<Expression>();
            while (CurrentIs("Ident") || CurrentIsLiteral() ||
                   CurrentIs("LeftParen") || CurrentIsOperator("-") ||
                   CurrentIsOperator("!") || CurrentIs("LeftCurl") ||
                   CurrentIs("LeftBracket"))
            {
                expressions.Add(Expression(table));
                if (!CurrentIs("RightBracket"))
                {
                    Expect("Comma");
                }
            }
            var rb = Expect("RightBracket").Segment;
            return new ArrayLiteral(expressions) {Segment = lb + rb};
        }
    }
}
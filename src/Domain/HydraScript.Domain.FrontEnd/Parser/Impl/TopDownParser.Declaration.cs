using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl;

public partial class TopDownParser
{
    /// <summary>
    /// Declaration -> LexicalDeclaration
    ///                FunctionDeclaration
    ///                TypeDeclaration
    /// </summary>
    private Declaration Declaration()
    {
        if (CurrentIsKeyword("function"))
        {
            return FunctionDeclaration();
        }

        if (CurrentIsKeyword("let") || CurrentIsKeyword("const"))
        {
            return LexicalDeclaration();
        }

        if (CurrentIsKeyword("type"))
        {
            return TypeDeclaration();
        }

        throw new ParserException(nameof(Declaration), _tokens.Current);
    }

    /// <summary>
    /// FunctionDeclaration -> 'function' "Ident" '(' FunctionParameters? ')' Type? BlockStatement
    /// </summary>
    private FunctionDeclaration FunctionDeclaration()
    {
        Expect("Keyword", "function");
        var ident = Expect("Ident");

        Expect("LeftParen");
        var args = new List<IFunctionArgument>();
        var indexOfFirstDefaultArgument = int.MaxValue;
        while (CurrentIs("Ident"))
        {
            var arg = Expect("Ident");
            if (CurrentIs("Colon"))
            {
                Expect("Colon");
                var type = TypeValue();
                args.Add(new NamedArgument(arg.Value, type));
            }
            else if (CurrentIs("Assign"))
            {
                Expect("Assign", "=");
                var value = LiteralNode();
                indexOfFirstDefaultArgument = args.Count < indexOfFirstDefaultArgument
                    ? args.Count
                    : indexOfFirstDefaultArgument;
                args.Add(new DefaultValueArgument(arg.Value, value));
            }
            else throw new ParserException($"Expected ':' or '=' after argument name <{arg}>");

            if (!CurrentIs("RightParen"))
                Expect("Comma");
        }

        Expect("RightParen");
        TypeValue returnType = TypeIdentValue.Undefined;

        if (CurrentIs("Colon"))
        {
            Expect("Colon");
            returnType = TypeValue();
        }

        var name = new IdentifierReference(ident.Value) { Segment = ident.Segment };
        return new FunctionDeclaration(name, returnType, args, BlockStatement(), indexOfFirstDefaultArgument)
            { Segment = ident.Segment };
    }

    /// <summary>
    /// LexicalDeclaration -> LetOrConst "Ident" Initialization (',' "Ident" Initialization)*
    /// </summary>
    private LexicalDeclaration LexicalDeclaration()
    {
        var readOnly = CurrentIsKeyword("const");
        Expect("Keyword", readOnly ? "const" : "let");
        var declaration = new LexicalDeclaration(readOnly);

        declaration.AddAssignment(DeclarationAssignmentExpression());

        while (CurrentIs("Comma"))
        {
            Expect("Comma");
            declaration.AddAssignment(DeclarationAssignmentExpression());
        }

        return declaration;
    }

    /// <summary>
    /// Initialization -> Typed | Initializer
    /// Typed -> Type Initializer?
    /// Initializer -> '=' Expression
    /// </summary>
    private AssignmentExpression DeclarationAssignmentExpression()
    {
        var ident = Expect("Ident");
        var identRef = new IdentifierReference(ident.Value) { Segment = ident.Segment };

        if (CurrentIs("Assign"))
        {
            var assignSegment = Expect("Assign", "=").Segment;
            return new AssignmentExpression(
                    new MemberExpression(identRef), Expression())
                { Segment = assignSegment };
        }

        if (CurrentIs("Colon"))
        {
            Expect("Colon");
            var type = TypeValue();
            var assignSegment = CurrentIs("Assign") ? Expect("Assign", "=").Segment : string.Empty;
            var expression = assignSegment is not "" ? Expression() : new ImplicitLiteral(type);
            return new AssignmentExpression(
                    new MemberExpression(identRef), expression, type)
                { Segment = assignSegment };
        }

        throw new ParserException($"Expected ':' or '=' after var name <{ident}>");
    }

    /// <summary>
    /// TypeDeclaration -> 'type' "Ident" = TypeValue
    /// </summary>
    private TypeDeclaration TypeDeclaration()
    {
        var typeWord = Expect("Keyword", "type");
        var ident = Expect("Ident");
        Expect("Assign", "=");
        var type = TypeValue();

        var typeId = new IdentifierReference(name: ident.Value)
            { Segment = ident.Segment };

        return new TypeDeclaration(typeId, type) { Segment = typeWord.Segment + ident.Segment };
    }

    /// <summary>
    /// TypeValue -> TypeValueBase TypeValueSuffix*
    /// </summary>
    private TypeValue TypeValue()
    {
        if (CurrentIs("Ident"))
        {
            var ident = Expect("Ident");
            var identType = new TypeIdentValue(
                TypeId: new IdentifierReference(name: ident.Value)
                    { Segment = ident.Segment });

            return WithSuffix(identType);
        }

        if (CurrentIs("LeftCurl"))
        {
            Expect("LeftCurl");
            var propertyTypes = new List<PropertyTypeValue>();
            while (CurrentIs("Ident"))
            {
                var ident = Expect("Ident");
                Expect("Colon");
                var propType = TypeValue();
                propertyTypes.Add(
                    new PropertyTypeValue(
                        ident.Value,
                        propType));
                Expect("SemiColon");
            }

            Expect("RightCurl");

            return WithSuffix(new ObjectTypeValue(propertyTypes));
        }

        throw new ParserException(nameof(TypeValue), _tokens.Current);
    }

    /// <summary>
    /// TypeValueSuffix -> '['']' | '?'
    /// </summary>
    private TypeValue WithSuffix(TypeValue baseType)
    {
        var type = baseType;
        while (CurrentIs("LeftBracket") || CurrentIs("QuestionMark"))
        {
            if (CurrentIs("LeftBracket"))
            {
                Expect("LeftBracket");
                Expect("RightBracket");
                type = new ArrayTypeValue(type);
            }
            else if (CurrentIs("QuestionMark"))
            {
                Expect("QuestionMark");
                type = new NullableTypeValue(type);
            }
        }

        return type;
    }
}
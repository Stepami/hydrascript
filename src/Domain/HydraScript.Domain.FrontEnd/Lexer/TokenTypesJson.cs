using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Domain.FrontEnd.Lexer;

public static class TokenTypesJson
{
    [StringSyntax(StringSyntaxAttribute.Json)]
    public const string String =
        """
        [
            {
                "tag": "Comment",
                "pattern": "[\/]{2}.*",
                "priority": 0,
                "canIgnore": true
            },
            {
                "tag": "Ident",
                "pattern": "[a-zA-Z][a-zA-Z0-9]*",
                "priority": 50
            },
            {
                "tag": "IntegerLiteral",
                "pattern": "[0-9]+",
                "priority": 3
            },
            {
                "tag": "FloatLiteral",
                "pattern": "[0-9]+[.][0-9]+",
                "priority": 2
            },
            {
                "tag": "NullLiteral",
                "pattern": "(?<![a-zA-Z0-9])(null)(?![a-zA-Z0-9])",
                "priority": 4
            },
            {
                "tag": "BooleanLiteral",
                "pattern": "(?<![a-zA-Z0-9])(true|false)(?![a-zA-Z0-9])",
                "priority": 5
            },
            {
                "tag": "StringLiteral",
                "pattern": "\\\"(\\\\.|[^\"\\\\])*\\\"",
                "priority": 6
            },
            {
                "tag": "Keyword",
                "pattern": "(?<![a-zA-Z0-9])(let|const|function|if|else|while|break|continue|return|as|type)(?![a-zA-Z0-9])",
                "priority": 11
            },
            {
                "tag": "Print",
                "pattern": "[>]{3}",
                "priority": 12
            },
            {
                "tag": "Operator",
                "pattern": "[+]{1,2}|[-]|[*]|[\/]|[%]|([!]|[=])[=]|([<]|[>])[=]?|[!]|[|]{2}|[&]{2}|[~]|[:]{2}",
                "priority": 13
            },
            {
                "tag": "Comma",
                "pattern": "[,]",
                "priority": 100
            },
            {
                "tag": "Dot",
                "pattern": "[.]",
                "priority": 105
            },
            {
                "tag": "LeftCurl",
                "pattern": "[{]",
                "priority": 101
            },
            {
                "tag": "RightCurl",
                "pattern": "[}]",
                "priority": 102
            },
            {
                "tag": "LeftParen",
                "pattern": "[(]",
                "priority": 103
            },
            {
                "tag": "RightParen",
                "pattern": "[)]",
                "priority": 104
            },
            {
                "tag": "LeftBracket",
                "pattern": "[[]",
                "priority": 107
            },
            {
                "tag": "RightBracket",
                "pattern": "[]]",
                "priority": 109
            },
            {
                "tag": "Assign",
                "pattern": "[=]",
                "priority": 99
            },
            {
                "tag": "QuestionMark",
                "pattern": "[?]",
                "priority": 90
            },
            {
                "tag": "Colon",
                "pattern": "[:]",
                "priority": 91
            },
            {
                "tag": "SemiColon",
                "pattern": "[;]",
                "priority": 92
            }
        ]
        """;
}
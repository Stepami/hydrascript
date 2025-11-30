namespace HydraScript.Domain.Constants;

public static class TokenTypes
{
    public record struct Dto(
        string Tag,
        string Pattern,
        int Priority,
        bool CanIgnore = false);

    public static IEnumerable<Dto> Stream
    {
        get
        {
            yield return new(
                Tag: "DoubleSlashComment",
                Pattern: "[/]{2}.*",
                Priority: 0,
                CanIgnore: true);

            yield return new(
                Tag: "ShebangComment",
                Pattern: "[#].*",
                Priority: 1,
                CanIgnore: true);

            yield return new(
                Tag: "Ident",
                Pattern: "[a-zA-Z][a-zA-Z0-9_]*",
                Priority: 50);

            yield return new(
                Tag: "IntegerLiteral",
                Pattern: "[0-9]+",
                Priority: 3);

            yield return new(
                Tag: "FloatLiteral",
                Pattern: "[0-9]+[.][0-9]+",
                Priority: 2);

            yield return new(
                Tag: "NullLiteral",
                Pattern: "(?<![a-zA-Z0-9])(null)(?![a-zA-Z0-9])",
                Priority: 4);

            yield return new(
                Tag: "BooleanLiteral",
                Pattern: "(?<![a-zA-Z0-9])(true|false)(?![a-zA-Z0-9])",
                Priority: 5);

            yield return new(
                Tag: "StringLiteral",
                Pattern: """
                         \"(\\.|[^"\\])*\"
                         """,
                Priority: 6);

            yield return new(
                Tag: "Keyword",
                Pattern:
                "(?<![a-zA-Z0-9])(let|const|function|if|else|while|break|continue|return|as|type|with)(?![a-zA-Z0-9])",
                Priority: 11);

            yield return new(
                Tag: "Print",
                Pattern: "[>]{3}",
                Priority: 12);

            yield return new(
                Tag: "Operator",
                Pattern: "[+]{1,2}|[-]|[*]|[/]|[%]|([!]|[=])[=]|([<]|[>])[=]?|[!]|[|]{2}|[&]{2}|[~]|[:]{2}|[$]",
                Priority: 13);

            yield return new(
                Tag: "Comma",
                Pattern: "[,]",
                Priority: 100);

            yield return new(
                Tag: "Dot",
                Pattern: "[.]",
                Priority: 105);

            yield return new(
                Tag: "LeftCurl",
                Pattern: "[{]",
                Priority: 101);

            yield return new(
                Tag: "RightCurl",
                Pattern: "[}]",
                Priority: 102);

            yield return new(
                Tag: "LeftParen",
                Pattern: "[(]",
                Priority: 103);

            yield return new(
                Tag: "RightParen",
                Pattern: "[)]",
                Priority: 104);

            yield return new(
                Tag: "LeftBracket",
                Pattern: "[[]",
                Priority: 107);

            yield return new(
                Tag: "RightBracket",
                Pattern: "[]]",
                Priority: 109);

            yield return new(
                Tag: "Assign",
                Pattern: "[=]",
                Priority: 99);

            yield return new(
                Tag: "QuestionMark",
                Pattern: "[?]",
                Priority: 90);

            yield return new(
                Tag: "Colon",
                Pattern: "[:]",
                Priority: 91);

            yield return new(
                Tag: "SemiColon",
                Pattern: "[;]",
                Priority: 92);
        }
    }
}
using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer;

namespace HydraScript.Infrastructure;

[PatternContainer<GeneratedRegexContainer>(TokenTypesJson.String)]
internal partial class GeneratedRegexContainer : IGeneratedRegexContainer
{
    [GeneratedRegex("""(?<Comment>[/]{2}.*)|(?<FloatLiteral>[0-9]+[.][0-9]+)|(?<IntegerLiteral>[0-9]+)|(?<NullLiteral>null)|(?<BooleanLiteral>true|false)|(?<StringLiteral>\"(\\.|[^"\\])*\")|(?<Keyword>let|const|function|if|else|while|break|continue|return|as|type)|(?<Operator>[+]{1,2}|[-]|[*]|[/]|[%]|([!]|[=])[=]|([<]|[>])[=]?|[!]|[|]{2}|[&]{2}|[~]|[:]{2})|(?<Ident>[a-zA-Z][a-zA-Z0-9]*)|(?<QuestionMark>[?])|(?<Colon>[:])|(?<SemiColon>[;])|(?<Assign>[=])|(?<Comma>[,])|(?<LeftCurl>[{])|(?<RightCurl>[}])|(?<LeftParen>[(])|(?<RightParen>[)])|(?<Dot>[.])|(?<LeftBracket>[[])|(?<RightBracket>[]])|(?<ERROR>\S+)""")]
    public static partial Regex GetRegex();
}
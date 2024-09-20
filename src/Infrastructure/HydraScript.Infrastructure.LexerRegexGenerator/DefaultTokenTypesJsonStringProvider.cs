using HydraScript.Domain.FrontEnd.Lexer;

namespace HydraScript.Infrastructure.LexerRegexGenerator;

internal class DefaultTokenTypesJsonStringProvider :
    ITokenTypesJsonStringProvider
{
    public string TokenTypesJsonString => TokenTypesJson.String;
}
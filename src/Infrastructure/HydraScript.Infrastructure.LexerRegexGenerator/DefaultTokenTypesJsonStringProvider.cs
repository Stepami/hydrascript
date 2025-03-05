using HydraScript.Domain.Constants;

namespace HydraScript.Infrastructure.LexerRegexGenerator;

internal class DefaultTokenTypesJsonStringProvider :
    ITokenTypesJsonStringProvider
{
    public string TokenTypesJsonString => TokenTypesJson.String;
}
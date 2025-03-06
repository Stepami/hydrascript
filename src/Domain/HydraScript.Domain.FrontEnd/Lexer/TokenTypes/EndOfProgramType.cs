using HydraScript.Domain.Constants;

namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

internal record EndOfProgramType() : TokenType(Eop.Tag);
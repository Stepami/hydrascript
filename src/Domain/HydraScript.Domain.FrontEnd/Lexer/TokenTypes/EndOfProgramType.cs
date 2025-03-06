using HydraScript.Domain.Constants;

namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

public record EndOfProgramType() : TokenType(Eop.Tag);
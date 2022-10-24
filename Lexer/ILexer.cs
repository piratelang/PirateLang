using Lexer.Tokens;
using Common;

namespace Lexer;

public interface ILexer
{
    ILogger Logger { get; set; }
    string fileName { get; set; }

    (List<Token> tokens, Error error) MakeTokens(string Text, string FileName);
}
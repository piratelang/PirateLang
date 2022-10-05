using System.Globalization;
using System.Runtime.CompilerServices;
using Common;
using Common.Enum;
using PirateLexer.Models;

namespace PirateParser;

public class Parser
{
    public static List<Token> tokenList { get; set; }
    public string writePath { get; set; }
    public int indentation { get; set; }
    public static Token currentToken { get; set; }
    public Logger logger { get; set; }

    public Parser(List<Token> TokenList, Logger Logger)
    {
        tokenList = TokenList;
        currentToken = tokenList[0];
        logger = Logger;
    }
    public static void Advance()
    {
        var index = tokenList.IndexOf(currentToken) + 1;
        if (tokenList.IndexOf(currentToken) + 2 >= tokenList.Count())
        {
            currentToken = null;
        }
        else
        {
            currentToken = tokenList[index];
        }
    }
    public bool Parse(string location, string fileName)
    {
        bool exists = System.IO.Directory.Exists(location);
        if (!exists)
            logger.Log("Creating Directory", this.GetType().Name, LogType.INFO);
            System.IO.Directory.CreateDirectory(location);


        logger.Log($"Creating {fileName}.py", this.GetType().Name, LogType.INFO);
        var file = File.CreateText($"./{location}/{fileName}.py");
        var writeRepository = new WriteRepository();


        logger.Log("Start Parsing Tokens", this.GetType().Name, LogType.INFO);
        while (currentToken != null)
        {
            var indentString = string.Empty;
            switch (currentToken.tokenType)
            {
                case TokenType.LEFTCURLYBRACE:
                    indentation += 1;
                    indentString = String.Concat(Enumerable.Repeat("    ", indentation));
                    WriteString(":\n" + indentString, file);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.RIGHTCURLYBRACE:
                    indentation -= 1;
                    indentString = String.Concat(Enumerable.Repeat("    ", indentation));
                    WriteString("\n" + indentString, file);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.DOUBLEDIVIDE:
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    while (currentToken.tokenType != TokenType.SEMICOLON)
                    {
                        Advance();
                    }
                    Advance();
                    continue;

                case TokenType.LEFTBRACKET:
                    WriteString("[", file, false, false);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.RIGHTBRACKET:
                    WriteString("]", file, false, false);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;

                case TokenType.SEMICOLON:
                    indentString = String.Concat(Enumerable.Repeat("    ", indentation));
                    WriteString("\n" + indentString, file);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;

                case TokenType.INT:
                    WriteString(currentToken.value.ToString(), file);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.FLOAT:
                    WriteString(currentToken.value.ToString(), file);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.STRING:
                    WriteString('"' + currentToken.value.ToString() + '"', file);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.IDENTIFIER:
                    WriteString(currentToken.value.ToString(), file);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;

                case TokenType.PLUS:
                    WriteString("+", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.PLUSEQUALS:
                    WriteString("+=", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.MINUS:
                    WriteString("-", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.MULTIPLY:
                    WriteString("*", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.DIVIDE:
                    WriteString("/", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.EQUALS:
                    WriteString("=", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.LEFTPARENTHESES:
                    WriteString("(", file);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.RIGHTPARENTHESES:
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    WriteString(")", file);
                    Advance();
                    continue;
                case TokenType.POWER:
                    WriteString("**", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.DOUBLEEQUALS:
                    WriteString("==", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.NOTEQUALS:
                    WriteString("!=", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.LESSTHAN:
                    WriteString("<", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.GREATERHAN:
                    WriteString(">", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.LESSTHANEQUALS:
                    WriteString("<=", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.GREATERTHANEQUALS:
                    WriteString(">=", file, true, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.COMMA:
                    WriteString(",", file, false, true);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.DOT:
                    WriteString(".", file, false, false);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.DOLLAR:
                    WriteString("f", file, false, false);
                    logger.Log($"Found and Parsed \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    Advance();
                    continue;
                case TokenType.BOOLEAN:
                    logger.Log($"Found \"{currentToken.tokenType.ToString()}\"", this.GetType().Name, LogType.INFO);
                    switch (currentToken.value)
                    {
                        case "True":
                            WriteString("True", file);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "true":
                            WriteString("True", file);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "False":
                            WriteString("False", file);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "false":
                            WriteString("False", file);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                    }
                    continue;

                case TokenType.KEYWORD:
                    switch (currentToken.value)
                    {
                        case "var":
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "and":
                            WriteString("and", file, true, true);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "or":
                            WriteString("or", file, true, true);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "if":
                            WriteString("if", file);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "elif":
                            WriteString("elif", file);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "else":
                            WriteString("else", file);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "to":
                            WriteString("to", file, true, true);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "while":
                            WriteString("while", file);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "func":
                            WriteString("def", file, false, true);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "import":
                            WriteString("import", file, false, true);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "class":
                            WriteString("class", file, false, true);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                        case "for":
                            writeRepository.WriteForLoop(file, logger);
                            continue;
                        case "foreach":
                            writeRepository.WriteForeachLoop(file, logger);
                            continue;
                        case "in":
                            WriteString("in", file, true, true);
                            logger.Log($"Found and Parsed \"{currentToken.value}\"", this.GetType().Name, LogType.INFO);
                            Advance();
                            continue;
                    }
                    Advance();
                    continue;
                default:
                    logger.Log($"TokenType \"{currentToken.value}\" was not able to be Parser", this.GetType().Name, LogType.ERROR);
                    throw new Exception("Token not found: " + currentToken.tokenType);
            }
        }
        file.Close();
        logger.Log("Finished Parsing\n", this.GetType().Name, LogType.INFO);
        return true;
    }

    public static void WriteString(string input, StreamWriter file, bool spaceBefore = false, bool spaceAfter = false)
    {
        if (spaceBefore)
        {
            file.Write(" ");
        }
        foreach (var item in input)
        {
            file.Write(item);
        }
        if (spaceAfter)
        {
            file.Write(" ");
        }
    }


}
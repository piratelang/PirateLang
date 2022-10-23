using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parser.Node.Interfaces;
using Lexer.Tokens;
using Lexer.Enums;

namespace Parser.Node;

[Serializable]
public class VariableAssignNode : INode
{
    public Token TypeToken { get; set; }
    public IValueNode Identifier { get; set; }
    public INode Value { get; set; }

    public VariableAssignNode(Token typeToken, IValueNode identifier, INode value)
    {
        TypeToken= typeToken;
        Identifier = identifier;
        Value = value;
    }

    public override string ToString()
    {
        return $"({Identifier.ToString()} = {Value.ToString()})";
    }

    public bool IsValid()
    {
        if (TypeToken.TokenType is not TokenTypeKeyword)
        {
            return false;
        }
        if (Identifier is not IValueNode)
        {
            return false;
        }
        if (Value is not INode)
        {
            return false;
        }
        return true;
    }
}
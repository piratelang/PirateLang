using NewPirateLexer.Tokens;
using NewParserTest.Node.Interfaces;

namespace NewParserTest.Node;

public class ComparisonOperationNode : INode, IOperationNode
{
    public INode Left { get; set; }
    public Token Operator { get; set; }
    public INode Right { get; set; }

    public ComparisonOperationNode(INode left, Token operatorToken, INode right)
    {
        Left = left;
        Operator = operatorToken;
        Right = right;
    }

    public string Display()
    {
        return $"{Left.Display()} | {Operator.Display()} | {Right.Display()}";
    }
}
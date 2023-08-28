
using Pirate.Common.Interfaces;
using Pirate.Interpreter.Values;
using Pirate.Interpreter.Values;
using Pirate.Parser.Node.Interfaces;

namespace Pirate.Interpreter.Interpreters;

/// <summary>
/// Converts the function declaration node to a function value.
/// </summary>
public class FunctionDeclarationInterpreter : BaseInterpreter
{
    public IFunctionDeclarationNode FunctionDeclarationNode { get; set; }

    public FunctionDeclarationInterpreter(INode node, InterpreterFactory InterpreterFactory, ILogger logger) : base(logger, InterpreterFactory)
    {
        if (node is not IFunctionDeclarationNode) throw new TypeConversionException(node.GetType(), typeof(IFunctionDeclarationNode));
        FunctionDeclarationNode = (IFunctionDeclarationNode)node;

        Logger.Log($"Created {GetType().Name} : \"{FunctionDeclarationNode.ToString()}\"", LogType.INFO);
    }

    public override List<BaseValue> VisitNode()
    {
        Logger.Log($"Visiting {GetType().Name} : \"{FunctionDeclarationNode.ToString()}\"", LogType.INFO);

        var function = new FunctionValue(FunctionDeclarationNode, Logger);
        SymbolTable.Instance(Logger).SetBaseValue((string)FunctionDeclarationNode.Identifier.Value.Value, function);
        return new List<BaseValue>();
    }
}
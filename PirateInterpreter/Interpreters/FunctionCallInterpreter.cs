using PirateInterpreter.StandardLibrary;
using PirateInterpreter.Values;

namespace PirateInterpreter.Interpreters;

public class FunctionCallInterpreter : BaseInterpreter
{
    public IFunctionCallNode functionCallNode { get; set; }
    private StandardLibraryFactory StandardLibraryFactory;
    private List<string> LibraryList = new() { "IO" };

    public FunctionCallInterpreter(INode node, InterpreterFactory InterpreterFactory, ILogger logger, StandardLibraryFactory standardLibraryFactory) : base(logger, InterpreterFactory)
    {
        if (node is not IFunctionCallNode) throw new TypeConversionException(node.GetType(), typeof(IFunctionCallNode));
        functionCallNode = (IFunctionCallNode)node;
        StandardLibraryFactory = standardLibraryFactory;

        Logger.Log($"Created {this.GetType().Name} : \"{functionCallNode.ToString()}\"", Common.Enum.LogType.INFO);
    }

    public override List<BaseValue> VisitNode()
    {
        Logger.Log($"Visiting {this.GetType().Name} : \"{functionCallNode.ToString()}\"", Common.Enum.LogType.INFO);

        var functionCallName = (string)functionCallNode.Identifier.Value.Value;
        if (LibraryList.Contains(functionCallName.Split('.')[0]))
        {
            return CallLibraryFunction(functionCallName.Split('.'), functionCallName);
        }

        var functionValue = SymbolTable.Instance(Logger).GetBaseValue((string)functionCallNode.Identifier.Value.Value);
        if (functionValue is not FunctionValue) throw new TypeConversionException(functionValue.GetType(), typeof(FunctionValue));
        var function = (FunctionValue)functionValue;

        foreach (var (parameter, value) in function.functionDeclarationNode.Parameters.Zip(functionCallNode.Parameters))
        {
            SymbolTable.Instance(Logger).SetBaseValue((string)parameter.Identifier.Value.Value, InterpreterFactory.GetInterpreter(value, Logger).VisitSingleNode());
        }

        foreach (var node in function.functionDeclarationNode.Statements)
        {
            var interpreter = InterpreterFactory.GetInterpreter(node, Logger);
            interpreter.VisitNode();
        }

        var resultList = new List<BaseValue>();
        if (function.functionDeclarationNode.ReturnNode is not null)
        {
            resultList.Add(InterpreterFactory.GetInterpreter(function.functionDeclarationNode.ReturnNode, Logger).VisitSingleNode());
        }
        return resultList;
    }

    private List<BaseValue> CallLibraryFunction(string[] splitidentifier, string functionCallName)
    {
        var libraryValue = SymbolTable.Instance(Logger).GetBaseValue(functionCallName.Split('.')[0]);
        if (splitidentifier.Count() > 2) throw new InvalidOperationException("Cannot call a function in a library in a library");
        var library = (Library)libraryValue;
        List<BaseValue> parameters = new List<BaseValue>();
        foreach (var parameter in functionCallNode.Parameters)
        {
            var parameterInterpreter = InterpreterFactory.GetInterpreter(parameter, Logger);
            parameters.AddRange(parameterInterpreter.VisitNode());
        }
        return new List<BaseValue>() { StandardLibraryFactory.GetFunction(splitidentifier[0], splitidentifier[1], parameters, Logger) };
    }
}
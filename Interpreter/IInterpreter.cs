using Interpreter.Values;
using Common;

namespace Interpreter;

public interface IInterpreter
{
    ILogger Logger { get; set; }

    List<BaseValue> StartInterpreter(string filename);
}
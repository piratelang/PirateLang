using Common;
using Common.Enum;
using Common.Interfaces;
using Interpreter;
using Shell.Commands.Interfaces;

namespace Shell.Commands;

public class RunCommand : Command, ICommand, IRunCommand
{
    public IObjectSerializer ObjectSerializer;
    public IBuildCommand BuildCommand;
    public IInterpreter Interpreter { get; set; }
    public string Location = EnvironmentVariables.GetVariable("location");
    public RunCommand(ILogger logger, IObjectSerializer objectSerializer, IBuildCommand buildCommand, IInterpreter interpreter) : base(logger)
    {
        ObjectSerializer = objectSerializer;
        BuildCommand = buildCommand;
        Interpreter = interpreter;
    }
    public override void Run(string[] arguments)
    {
        Logger.Log("Starting Run Command", this.GetType().Name, LogType.INFO);
        var fileArgument = "main";
        if (arguments.Length >= 2) { fileArgument = arguments[1]; }
        var fileName = fileArgument.Replace(".pirate", "");

        if (!File.Exists($"./{fileName}.pirate")) Error($"File \"{fileArgument}\" not provided or does not exist.");

        Logger.Log("Starting build", this.GetType().Name, LogType.INFO);
        BuildCommand.Run(arguments);
        Logger.Log("Completed Build", this.GetType().Name, LogType.INFO);

        Logger.Log($"Executing {fileName}.pirate\n", this.GetType().Name, LogType.INFO);

        var interpreterResult = Interpreter.StartInterpreter(fileName);
        foreach (var item in interpreterResult)
        {
            Console.WriteLine(item.Value);
        }
    }

    public override void Help()
    {
        Console.WriteLine(String.Join(
            Environment.NewLine,
            "Description",
            "   pirate run command",
            "\nUsage",
            "   pirate run [filename]",
            "\nOptions",
            "   -h --help   Show command line help."
        ));
    }
}

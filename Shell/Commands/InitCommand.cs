using Common;
using Common.Enum;

namespace Shell.Commands
{
    public class InitCommand : ICommand
    {
        public string version { get; set; }
        public Logger logger { get; set; }
        public InitCommand(string Version, Logger Logger)
        {
            version = Version;
            logger = Logger;
        }
        public void Run(string[] arguments)
        {
            logger.Log("Starting Init Command", this.GetType().Name, LogType.INFO);
            var nameArgument = "main";
            if (arguments.Length == 2) { nameArgument = arguments[1];}

            logger.Log($"Creating {nameArgument} file", this.GetType().Name, LogType.INFO);
            var fileName = nameArgument.Replace(".pirate", "");
            var file = File.CreateText($"./{fileName}.pirate");
            file.Write(String.Join(
                Environment.NewLine,
                "func main()",
                "{",
                "    print(\"Hello World\");",
                "}"
            ));
            file.Close();
            logger.Log($"Created {nameArgument} file", this.GetType().Name, LogType.INFO);
            Console.WriteLine($"\nCreated {fileName}.pirate");
        }

        public void Help()
        {
            Console.WriteLine(String.Join(
                Environment.NewLine,
                "Description",
                "   pirate initalize command",
                "\nUsage",
                "   pirate init [filename]",
                "\nOptions",
                "   -h --help       Show command line help."
            ));
        }
        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
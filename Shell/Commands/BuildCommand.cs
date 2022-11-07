using Common;
using PirateParser;
using Shell.ModuleList;
using Shell.Commands.Interfaces;
using PirateLexer.Interfaces;

namespace Shell.Commands;

public class BuildCommand : Command, ICommand, IBuildCommand
{
    private IObjectSerializer _objectSerializer;
    private IParser _parser;
    private ILexer _lexer;
    private IModuleListRepository _moduleListRepository;
    private IFileReadHandler _fileReadHandler;
    private string Location = EnvironmentVariables.GetVariable("location");

    public BuildCommand(ILogger logger, IObjectSerializer objectSerializer, IParser parser, ILexer lexer, IModuleListRepository moduleListRepository, IFileReadHandler fileReadHandler) : base(logger)
    {
        _objectSerializer = objectSerializer;
        _parser = parser;
        _lexer = lexer;
        _moduleListRepository = moduleListRepository;
        _fileReadHandler = fileReadHandler;
    }
    
    public override void Run(string[] arguments)
    {
        Logger.Log("Starting Build Command", this.GetType().Name, LogType.INFO);

        // Check for files
        var foundFiles = Directory.GetFiles("./", "*.pirate", SearchOption.AllDirectories);
        if (foundFiles.Length == 0) Error("No files were found in the directory");

        var moduleList = _moduleListRepository.GetList(Location);

        foreach (var file in foundFiles)
        {
            if (CheckModuleList(moduleList, file)) break;

            // Starting build
            Console.WriteLine($"Building {file}\n");
            Logger.Log($"Building {file}", this.GetType().Name, LogType.INFO);

            var fileName = file.Replace(".pirate", "").Replace("./", "");
            
            var text = _fileReadHandler.ReadAllTextFromFile(fileName, ".pirate", "").Result;
            if (text == null) Error($"{fileName} contains no text");

            // Running Lexer
            Logger.Log($"Lexing {file}\n", this.GetType().Name, LogType.INFO);
            var tokens = _lexer.MakeTokens(text, "test");
            if (tokens.Count() == 0) Error($"Error occured while lexing tokens, in the file {fileName}.");

            // Running Parser
            Logger.Log($"Parsing {file}\n", this.GetType().Name, LogType.INFO);
            var parseResult = _parser.StartParse(tokens, fileName);
            if (parseResult.Nodes.Count() < 1) Error("Error occured while parsing tokens.");
        }
        Logger.Log($"Updating ModuleList\n", this.GetType().Name, LogType.INFO);
        _moduleListRepository.SetList(foundFiles, Location);
    }
    
    public override void Help()
    {
        Console.WriteLine(String.Join(
            Environment.NewLine,
            "Description",
            "   pirate project building command",
            "\nUsage",
            "   pirate build",
            "\nOptions",
            "   -h --help       Show command line help."
        ));
    }

    public bool CheckModuleList(List<Module> moduleList, string file)
    {
        if (moduleList == null) return false;
        var foundModule = moduleList.Where(a => a.moduleName == file.Replace("./", "")).FirstOrDefault();
        if (foundModule != null)
        {
            if (
                foundModule.moduleName == File.OpenRead(file).Name.Split("\\").Last() &&
                foundModule.path == File.OpenRead(file).Name &&
                foundModule.lastModifiedDate == File.GetLastWriteTimeUtc(file)
            )
            {
                Logger.Log($"{foundModule.moduleName} was not modified since last build", this.GetType().Name, LogType.INFO);
                return true;
            }
        }
        return false;
    }
}

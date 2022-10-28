﻿using Common;

Console.WriteLine("Hello, World!");
var Logger = new Logger("Test");

while (true)
{
    var input = Console.ReadLine();
    var tokens = Lexer.Lexer.Instance(Logger).MakeTokens(input, "test");

    ObjectSerializer objectSerializer = new(Logger);

    var parser = new Parser.Parser(Logger, objectSerializer);
    var parseResult = parser.StartParse(tokens, "Test");

    if (parseResult.Nodes == null)
    {
        Console.WriteLine("stuk");
        return;
    }

    var interpreter = new Interpreter.Interpreter(objectSerializer, Logger);
    var Result = interpreter.StartInterpreter("Test");

    if (Result == null)
    {
        Console.WriteLine("Why is this null?");
        return;
    }
    foreach (var item in Result)
    {
        Console.WriteLine(item.Value);
    }
}

using CommandLine;
namespace ConsoleClient;

public class Options
{
    [Option('i', "input", Default = "example.txt", HelpText = "Путь к входному текстовому файлу.")]
    public required string InputFilePath { get; set; }

    [Option('o', "output", Default = "example.png", HelpText = "Путь к выходному изображению.")]
    public required string OutputFilePath { get; set; }

    [Option('x', "centerX", Default = 200, HelpText = "Координата X центра.")]
    public int CenterX { get; set; }

    [Option('y', "centerY", Default = 200, HelpText = "Координата Y центра.")]
    public int CenterY { get; set; }
}
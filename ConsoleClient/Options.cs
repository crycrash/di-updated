using CommandLine;
namespace ConsoleClient;

public class Options
{
    [Option('i', "input", Default = "Examples/example.txt", HelpText = "Путь к входному текстовому файлу.")]
    public required string InputFilePath { get; set; }

    [Option('o', "output", Default = "Examples/example.png", HelpText = "Путь к выходному изображению.")]
    public required string OutputFilePath { get; set; }

    [Option('x', "centerX", Default = 200, HelpText = "Координата X центра.")]
    public int CenterX { get; set; }

    [Option('y', "centerY", Default = 200, HelpText = "Координата Y центра.")]
    public int CenterY { get; set; }

    [Option('l', "lenght", Default = 400, HelpText = "Длина изображения")]
    public int Lenght { get; set; }

    [Option('w', "width", Default = 400, HelpText = "Ширина изображения")]
    public int Width{ get; set; }

    [Option('c', "color", Default = "white", HelpText = "Цвет текста")]
    public string Color{ get; set; }
}
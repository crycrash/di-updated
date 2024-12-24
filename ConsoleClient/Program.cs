using System.Drawing;
using Autofac;
using CommandLine;
using DrawingTagsCloudVisualization;
using TagsCloudVisualization;
using TagsCloudVisualization.FilesProcessing;
using TagsCloudVisualization.ManagingRendering;

namespace ConsoleClient;

public class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunApplication)
            .WithNotParsed(HandleErrors);
    }

    private static void RunApplication(Options options)
    {
        var builder = new ContainerBuilder();

        builder.Register(c =>
        {
            var mystem = new MyStemWrapper.MyStem
            {
                PathToMyStem = "mystem",
                Parameters = "-ni"
            };
            return mystem;
        }).As<MyStemWrapper.MyStem>().SingleInstance();

        builder.RegisterType<MorphologicalProcessing>()
            .As<IMorphologicalAnalyzer>()
            .InstancePerDependency();

        builder.RegisterType<TxtFileProcessor>()
            .As<IFileProcessor>()
            .InstancePerDependency();

        builder.RegisterType<RectangleGenerator>()
            .As<IRectangleGenerator>()
            .InstancePerDependency();

        builder.Register<ISpiral>(c =>
        {
            if (options.Algorithm.Equals("Circle", StringComparison.OrdinalIgnoreCase))
            {
                return new ArchimedeanSpiral(new Point(options.CenterX, options.CenterY), 1);
            }
            return new FermatSpiral(new Point(options.CenterX, options.CenterY), 20);
        }).As<ISpiral>().InstancePerDependency();

        builder.RegisterType<WordHandler>()
            .As<IWordHandler>()
            .InstancePerDependency();

        builder.Register(c =>
        {
            var wordHandler = c.Resolve<IWordHandler>();
            var frequencyRectangles = wordHandler.ProcessFile(options.InputFilePath);
            var arrRect = c.Resolve<IRectangleGenerator>().ExecuteRectangles(frequencyRectangles, new Point(options.CenterX, options.CenterY));
            return new DrawingTagsCloud(arrRect);
        }).As<IDrawingTagsCloud>().InstancePerDependency();

        var container = builder.Build();

        using var scope = container.BeginLifetimeScope();
        try
        {
            var drawingTagsCloud = scope.Resolve<IDrawingTagsCloud>();
            drawingTagsCloud.SaveToFile(options.OutputFilePath, options.Lenght, options.Width, options.Color);

            Console.WriteLine($"Облако тегов успешно сохранено в файл: {options.OutputFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    private static void HandleErrors(IEnumerable<Error> errors)
    {
        Console.WriteLine("Ошибка при обработке аргументов командной строки.");
    }
}

using Autofac;
using CommandLine;

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
        if (options.AlgorithmForming != "Circle" && options.AlgorithmForming != "Fermat")
        {
            Console.WriteLine($"Ошибка: Неизвестный алгоритм '{options.AlgorithmForming}'. Допустимые значения: 'Circle', 'Fermat'.");
            return;
        }

        if (options.PartOfSpeech != "all" && options.PartOfSpeech != "S" && options.PartOfSpeech != "V"
            && options.PartOfSpeech != "A")
        {
            Console.WriteLine($"Ошибка: Неизвестная часть речи '{options.PartOfSpeech}'");
            return;
        }

        if (options.AlgorithmDrawing != "Standart" && options.AlgorithmDrawing != "Altering")
        {
            Console.WriteLine($"Ошибка: Неизвестный алгоритм рассказки '{options.AlgorithmDrawing}'");
            return;
        }
        var container = DependencyInjectionConfig.BuildContainer(options);

        using var scope = container.BeginLifetimeScope();
        try
        {
            scope.Resolve<ITagsCloudDrawingFacade>().DrawRectangle(options);
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
        foreach (var error in errors)
        {
            switch (error)
            {
                case UnknownOptionError unknownOptionError:
                    Console.WriteLine($"- Неизвестный параметр: {unknownOptionError.Token}");
                    break;
                case SetValueExceptionError setValueExceptionError:
                    Console.WriteLine($"- Ошибка установки значения: {setValueExceptionError.Exception.Message}");
                    break;
                case MissingValueOptionError missingValueOptionError:
                    Console.WriteLine($"- Отсутствует значение: {missingValueOptionError.NameInfo}");
                    break;
                default:
                    Console.WriteLine($"- Неизвестная ошибка: {error.GetType().Name}");
                    break;
            }
        }
    }
}

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
        if (options.Algorithm != "Circle" && options.Algorithm != "Fermat")
        {
            Console.WriteLine($"Ошибка: Неизвестный алгоритм '{options.Algorithm}'. Допустимые значения: 'Circle', 'Fermat'.");
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

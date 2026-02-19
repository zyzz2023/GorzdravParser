using GorzdravParser.Application;
using GorzdravParser.Application.Intefaces;
using GorzdravParser.Core.Common;
using GorzdravParser.Core.Common.Interfaces;
using GorzdravParser.Infrastructure.Services;

namespace GorzdravParser;

class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            IParserSettings settings = new ParserSettings();
            IParser parser = new Parser();
            IParserWorker worker = new ParserWorker(parser, settings);
            ISaveToCsvService csvService = new SaveToCsvService();
            const string FileName = "Results.csv";

            Console.WriteLine("Начинаем парсинг...\n");

            var medications = await worker.Worker();

            Console.WriteLine($"Найдено товаров: {medications.Count}\n");

            csvService.SaveToCsv(FileName, medications);

            foreach (var item in medications)
            {
                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Название: {item.Name}");
                Console.WriteLine($"Рецептурность: {item.Prescription}");
                Console.WriteLine($"Действующее вещество: {item.ActiveIngredient}");
                Console.WriteLine($"Цена: {item.Price}");
                Console.WriteLine($"Цена старая: {item.OldPrice}");
                Console.WriteLine($"Производитель: {item.Manufacturer}");
                Console.WriteLine($"Ссылка на картинку: {item.PictureUrl}");
                Console.WriteLine($"Ссылка на товар: {item.ProductUrl}");
                Console.WriteLine($"Регион: {item.Region}");
                Console.WriteLine(new string('-', 60));
            }

            Console.WriteLine("Парсинг завершён.");

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка выполнения:");
            Console.WriteLine(ex.Message);
            return 1;
        }
    }
}

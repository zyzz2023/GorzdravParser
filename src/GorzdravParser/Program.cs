using GorzdravParser.Application;
using GorzdravParser.Core.Common;

namespace GorzdravParser;

class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var settings = new ParserSettings
            {
                BaseUrl = "https://gorzdrav.org",
                Url = "https://gorzdrav.org/category/sredstva-ot-diabeta",
                Prefix = "page/{CurrentPage}"
            };

            IParser parser = new Parser();
            var worker = new ParserWorker(parser, settings);

            Console.WriteLine("Начинаем парсинг...\n");

            var medications = await worker.Worker();

            Console.WriteLine($"Найдено товаров: {medications.Count}\n");

            foreach (var item in medications)
            {
                Console.WriteLine($"{item.Id} | {item.Name}");
                Console.WriteLine($"Цена: {item.Price}");
                Console.WriteLine($"Производитель: {item.Manufacturer}");
                Console.WriteLine(new string('-', 60));
            }

            Console.WriteLine("Парсинг завершён.");

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка выполнения:");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException);
            return 1;
        }
    }
}

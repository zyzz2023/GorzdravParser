using GorzdravParser.Application.Intefaces;
using GorzdravParser.Core.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Security;
using System.Text;

namespace GorzdravParser.Infrastructure.Services;

public class SaveToCsvService : ISaveToCsvService
{
    private readonly IWorkbook _workbook;
    private readonly ISheet _sheet;

    private readonly string _filePath;
    public SaveToCsvService(string filePath = null)
    {
        _workbook = new XSSFWorkbook();
        _sheet = _workbook.CreateSheet();

        if (string.IsNullOrEmpty(filePath))
        {
            var projectRoot = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..")
            );
            _filePath = Path.Combine(projectRoot, "Data", "Csv");
        }
        else
        {
            _filePath = filePath;
        }

        Directory.CreateDirectory(_filePath);
        Console.WriteLine($"CSV файлы будут сохраняться в: {_filePath}");
    }
    public void SaveToCsv(string fileName, List<MedicationRow> medications)
    {
        if (!fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            fileName += ".csv";

        var fullPath = Path.Combine(_filePath, fileName);

        using var writer = new StreamWriter(fullPath, false, Encoding.UTF8);

        writer.WriteLine("Id;Name;Prescription;Manufacturer;ActiveIngredient;Price;OldPrice;PictureUrl;ProductUrl;Region");

        // Записываем данные
        foreach (var med in medications)
        {
            var line = $"{med.Id};" +
                      $"{EscapeCsvValue(med.Name)};" +
                      $"{EscapeCsvValue(med.Prescription)};" +
                      $"{EscapeCsvValue(med.Manufacturer)};" +
                      $"{EscapeCsvValue(med.ActiveIngredient)};" +
                      $"{EscapeCsvValue(med.Price)};" +
                      $"{EscapeCsvValue(med.OldPrice)};" +
                      $"{EscapeCsvValue(med.PictureUrl)};" +
                      $"{EscapeCsvValue(med.ProductUrl)};" +
                      $"{EscapeCsvValue(med.Region)}";

            writer.WriteLine(line);
        }

        Console.WriteLine($"Данные сохранены в {fullPath}");
    }

    private string EscapeCsvValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        // Если запись содержит разделитель, кавычки или перенос строки, оборачиваем в кавычки
        if (value.Contains(";") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
        {
            value = value.Replace("\"", "\"\"");
            return $"\"{value}\"";
        }

        return value;
    }
}
using GorzdravParser.Core.Common.Interfaces;

namespace GorzdravParser.Core.Common;

public class ParserSettings : IParserSettings
{
    public string BaseUrl { get; set; } = "https://gorzdrav.org/";
    public string Url { get; set; } = "https://gorzdrav.org/category/sredstva-ot-diabeta";
    public int CountProducts { get; set; } = 25; // Количество для ожидания загрузки
}

using AngleSharp.Dom;

namespace GorzdravParser.Core.Common;

public interface IParserSettings
{
    public string BaseUrl { get; set; }
    public string Url { get; set; }
    public string Prefix { get; set; } // Пагинация
}

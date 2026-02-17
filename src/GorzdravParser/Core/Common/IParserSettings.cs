using AngleSharp.Dom;

namespace GorzdravParser.Core.Common;

public interface IParserSettings
{
    public string BaseUrl { get; set; }
    public string Url { get; set; }
    public int CountProducts { get; set; } 

}

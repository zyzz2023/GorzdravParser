using GorzdravParser.Application.Intefaces;
using GorzdravParser.Core.Common.Interfaces;
using GorzdravParser.Core.Models;

namespace GorzdravParser.Application;

public class ParserWorker : IParserWorker
{
    private readonly HtmlLoader _loader;
    private readonly IParser _parser;
    private readonly IParserSettings _settings;

    public ParserWorker(
        IParser parser,
        IParserSettings parserSettings)
    {
        _parser = parser;
        _settings = parserSettings;
        _loader = new HtmlLoader(_settings);
    }

    public async Task<List<MedicationRow>> Worker()
    {
        List<MedicationRow> rows = new List<MedicationRow>();
        int page = 1;
        bool isRunning = true;

        while(isRunning)
        {
            string source = _loader.GetSourceByCurrentPage(page);
            if (string.IsNullOrWhiteSpace(source))
                return rows;

            var result = _parser.Parse(source, _settings.BaseUrl);
            if (result != null && result.Count() > 0)
            {
                rows.AddRange(result);
            }
            else
            {
                isRunning = false;
            }
             
            page++;
        }

        return rows;
    }
}

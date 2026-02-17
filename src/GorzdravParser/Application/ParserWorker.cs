using AngleSharp.Dom;
using GorzdravParser.Core.Common;
using GorzdravParser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorzdravParser.Application;

public class ParserWorker
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

        while(page <= 3)
        {
            var source = _loader.GetSourceByCurrentPage(page);
            if (string.IsNullOrWhiteSpace(source))
                return rows;

            var result = _parser.Parse(source, _settings.BaseUrl);
            if (result != null && result.Count() > 0)
                rows.AddRange(result);
                
            page++;
        }

        return rows;
    }
}

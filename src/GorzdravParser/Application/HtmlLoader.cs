using GorzdravParser.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GorzdravParser.Application;

public class HtmlLoader
{
    private readonly HttpClient _httpClient;
    private readonly string _url;

    public HtmlLoader(IParserSettings settings)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent",
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
    "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

        _httpClient.DefaultRequestHeaders.Add("Accept",
            "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9");

        _httpClient.DefaultRequestVersion = HttpVersion.Version11;
        _httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
        _url = $"{settings.Url}/{settings.Prefix}";
    }

    public async Task<string> GetSourceByCurrentPage(int currentPage)
    {
        var currentUrl = _url.Replace("{CurrentPage}", currentPage.ToString());
        var response = await _httpClient.GetAsync(currentUrl);
        string source = string.Empty;

        if (response != null && response.StatusCode == HttpStatusCode.OK)
        {
            source = await response.Content.ReadAsStringAsync();
        }

        return source;
    }
}

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using GorzdravParser.Core.Common;
using GorzdravParser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorzdravParser.Application;

public class Parser : IParser
{
    public async Task<IEnumerable<MedicationRow>> Parse(string html, string baseUrl)
    {
        var context = BrowsingContext.New(Configuration.Default);
        var document = context.OpenAsync(req => req.Content(html)).Result;

        var result = new List<MedicationRow>();

        var cards = document.QuerySelectorAll(".product-card");

        foreach (var card in cards)
        {
            var titleElement = card.QuerySelector(".product-card-body__title");

            var name = titleElement?.TextContent;
            var hrevProductUrl = titleElement?.GetAttribute("href");

            // если отстутствует название или ссылка - пропускаем
            if (string.IsNullOrWhiteSpace(hrevProductUrl) || string.IsNullOrWhiteSpace(name)) 
                continue;

            var productUrl = new Uri(new Uri(baseUrl), hrevProductUrl).ToString();

            var hrefPictureUrl = card.QuerySelector(".product-card-image__img")?
                .GetAttribute("src");

            var pictureUrl = new Uri(new Uri(baseUrl), hrefPictureUrl).ToString();

            var prescription = card.QuerySelector(".custom-chip__text")?.TextContent;

            var manufacturer = ExtractField(card, "Производитель:");

            var activeIngredient = ExtractField(card, "Действующее вещество:");

            var region = ExtractField(card, "Страна:");

            var price = card.QuerySelector(".ui-price__price")?.TextContent;

            var oldPrice = card.QuerySelector(".ui-price__discount-value")?.TextContent;

            var id = ExtractIdFromUrl(hrevProductUrl);

            result.Add(MedicationRow.Create(
                id,
                name,
                prescription,
                manufacturer,
                activeIngredient,
                price,
                oldPrice,
                pictureUrl,
                productUrl,
                region));
        }

        return result;
    }

    private static string? ExtractField(IElement card, string fieldName)
    {
        var item = card.QuerySelectorAll(".product-card__item")
                .FirstOrDefault(x =>
                {
                    var label = x
                       .QuerySelector(".product-card__label")
                       ?.TextContent
                       ?.Replace(":", "")
                       ?.Trim();

                    return string.Equals(
                        label,
                        fieldName,
                        StringComparison.OrdinalIgnoreCase);
                });

        return item?
            .QuerySelector(".product-card__value")?
            .TextContent?
            .Replace(",","")?
            .Trim();
    }

    private static int ExtractIdFromUrl(string productUrl)
    {
        var trimmed = productUrl.TrimEnd('/');

        var lastDashIndex = trimmed.LastIndexOf('-');

        var idPart = trimmed[(lastDashIndex + 1)..];

        int.TryParse(idPart, out var id);

        return id;
    }

}

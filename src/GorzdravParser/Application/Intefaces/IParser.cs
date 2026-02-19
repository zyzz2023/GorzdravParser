using AngleSharp.Html.Dom;
using GorzdravParser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorzdravParser.Application.Intefaces;

public interface IParser
{
    IEnumerable<MedicationRow> Parse(string html, string baseUrl);
}

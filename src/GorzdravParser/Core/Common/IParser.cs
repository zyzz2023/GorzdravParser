using AngleSharp.Html.Dom;
using GorzdravParser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorzdravParser.Core.Common;

public interface IParser
{
    Task<IEnumerable<MedicationRow>> Parse(string html, string baseUrl);
}

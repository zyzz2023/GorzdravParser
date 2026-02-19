using GorzdravParser.Core.Models;

namespace GorzdravParser.Application.Intefaces;

public interface IParserWorker
{
    Task<List<MedicationRow>> Worker();
}

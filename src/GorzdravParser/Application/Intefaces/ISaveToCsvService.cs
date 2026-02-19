using GorzdravParser.Core.Models;

namespace GorzdravParser.Application.Intefaces;

public interface ISaveToCsvService
{
    void SaveToCsv(string fileName, List<MedicationRow> medications);
}

namespace GorzdravParser.Core.Models;

public class MedicationRow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Prescription { get; set; } // Рецептурность
    public string Manufacturer { get; set; }
    public string ActiveIngredient { get; set; }
    public string Price { get; set; }
    public string? OldPrice { get; set; }
    public string PictureUrl { get; set; }
    public string ProductUrl { get; set; }
    public string Region { get; set; }

    private MedicationRow(
        int id,
        string name,
        string? prescription,
        string manufacturer,
        string activeIngredient,
        string price,
        string? oldPrice,
        string pictureUrl,
        string productUrl,
        string region) 
    { 
        Id = id;
        Name = name;
        Prescription = prescription;
        Manufacturer = manufacturer;
        ActiveIngredient = activeIngredient;
        Price = price;
        OldPrice = oldPrice;
        PictureUrl = pictureUrl;
        ProductUrl = productUrl;
        Region = region;
    }

    public static MedicationRow Create(
        int id,
        string name,
        string? prescription,
        string manufacturer,
        string activeIngredient,
        string price,
        string? oldPrice,
        string pictureUrl,
        string productUrl,
        string region)
    {

        return new MedicationRow(
            id,
            name,
            prescription,
            manufacturer,
            activeIngredient,
            price,
            oldPrice,
            pictureUrl,
            productUrl,
            region);
    }


}

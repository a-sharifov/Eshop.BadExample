namespace eShop.MVC.Models.Classes;

public class FilterRequestFind
{
    public string? ProductName { get; set; }
    public string? ProductSeller { get; set; }
    public string? ProductBrand { get; set; }
    public string? ProductType { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = SD.TakeDefault;
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; } = SD.MaxPriceDefault;

    public FilterRequestFind() { }

    public FilterRequestFind(string productName) =>
        ProductName = productName;

}
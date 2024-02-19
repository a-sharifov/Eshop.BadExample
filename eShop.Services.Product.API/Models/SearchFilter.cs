namespace eShop.ServicesProduct.API.Models;

public class SearchFilter
{
    public int Skip { get; set; }
    public int Take { get; set; }
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public string? IncludeTables { get; set; }
    public string? IncludeFields { get; set; }
    public string? ProductType { get; set; }
    public string? ProductBrand { get; set; }
    public string? ProductName { get; set; }
    public string? ProductSeller { get; set; }
}

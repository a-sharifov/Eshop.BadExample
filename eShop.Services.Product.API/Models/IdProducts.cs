namespace eShop.ServicesProduct.API.Models;

public class IdProducts
{
    public IList<int> Ids { get; set; }
    public string? IncludeTables { get; set; }
    public string? IncludeFields { get; set; }
}
namespace eShop.MVC.ViewModels;

using System.ComponentModel.DataAnnotations;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Product Brand is required.")]
    public string ProductBrand { get; set; } = null!;

    [Required(ErrorMessage = "Product Type is required.")]
    public string ProductType { get; set; } = null!;

    [MaxLength(256, ErrorMessage = "Description max Length 256")]
    public string? Description { get; set; }

    [MaxLength(256, ErrorMessage = "Composition max Length 256")]
    public string? Composition { get; set; } 

    [Required(ErrorMessage = "Image URL is required.")]
    public string ImageUrl { get; set; } = null!;

    [Required(ErrorMessage = "Price is required.")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Count must be greater than 0.")]
    public int Count { get; set; }

    public string? ProductSeller { get; set; }  
}
#nullable disable

using System.ComponentModel.DataAnnotations;

namespace eShop.Services.Identity.API.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Nick name is required.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
    public string ConfirmPassword { get; set; }

    public string ReturnUrl { get; set; }

    [Required(ErrorMessage = "Role name is required.")]
    public string RoleName { get; set; }
}
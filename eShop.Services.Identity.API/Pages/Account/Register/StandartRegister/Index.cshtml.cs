using eShop.Services.Identity.API.Models;
using eShop.Services.Identity.API.Services.Interfaces;
using eShop.Services.Identity.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.Services.Identity.API.Pages.Account.Register.StandartRegister;

[AllowAnonymous]
public class IndexModel : PageModel
{

    public IndexModel(
        IApplicationUserManager userManager,
        SignInManager<ApplicationUser> signInManager) =>
        (_userManager, _signInManager) = (userManager, signInManager);

    private readonly IApplicationUserManager _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    [BindProperty]
    public RegisterViewModel Input { get; set; }

    public IActionResult OnGet(string returnUrl)
    {
        Input = new RegisterViewModel
        {
            ReturnUrl = returnUrl,
        };
        return Page();
    }

    public async Task<IActionResult> OnPost(string returnUrl)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser(Input);

            if (await _userManager.FindByEmailAsync(Input.Email) != null)
            {
                ModelState.AddModelError("DuplicateEmail", "User with this email already exists");
                return Page();
            }

            await CreateUserAsync(user);
            await _userManager.AddToRoleAsync(user, Input.RoleName);
            await _userManager.AddApplicationUserClaimsAsync(user);
            return await PasswordSignInAsync();
        }

        return Page();
    }

    private async Task CreateUserAsync(ApplicationUser user)
    {
        var result = await _userManager.CreateUserAsync(user, Input.Password);

        if (result.Errors.Any(x => x.Code == "DuplicateUserName"))
        {
            ModelState.AddModelError("DuplicateUserName", "User with this name already exists");
            return;
        }

        if (!result.Succeeded)
        {
            throw new Exception("User creation failed");
        }

        await _userManager.AddToRoleAsync(user, Input.RoleName);
        await _userManager.AddApplicationUserClaimsAsync(user);
    }

    private async Task<IActionResult> PasswordSignInAsync()
    {
        var loginResult = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, false, true);

        if (loginResult.Succeeded)
        {
            if (Url.IsLocalUrl(Input.ReturnUrl))
            {
                return Redirect(Input.ReturnUrl);
            }

            return string.IsNullOrEmpty(Input.ReturnUrl) ? Redirect("~/")
                : throw new Exception("Invalid return URL");
        }
        return Page();
    }
}
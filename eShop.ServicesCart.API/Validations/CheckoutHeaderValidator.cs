using eShop.ServicesCart.API.Models.Messages;
using FluentValidation;

namespace eShop.ServicesCart.API.Validations;

public class CheckoutHeaderValidator : AbstractValidator<CheckoutHeader>
{
    public CheckoutHeaderValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.CVV).NotEmpty().Length(3, 3);
        RuleFor(x => x.CardNumber).NotEmpty().CreditCard();
        RuleFor(x => x.CardExpiration).NotEmpty().GreaterThanOrEqualTo(DateTime.Today).WithMessage("Card is not valid.");
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.CartProducts).NotEmpty().WithMessage("Cart empty.");
    }
}

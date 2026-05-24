using FluentValidation;
using Ordering.Application.Orders.Requests;

namespace Ordering.Application.Orders.Validators;

public class PlaceOrderRequestValidator : AbstractValidator<PlaceOrderRequest>
{
    public PlaceOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
        RuleFor(x => x.Street).NotEmpty().WithMessage("Street is required.");
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
        RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.");
        RuleFor(x => x.Items).NotEmpty().WithMessage("Order must have at least one item.");
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId).NotEmpty();
            item.RuleFor(i => i.ProductName).NotEmpty();
            item.RuleFor(i => i.UnitPrice).GreaterThan(0);
            item.RuleFor(i => i.Quantity).GreaterThan(0);
        });
    }
}

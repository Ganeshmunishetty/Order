using FluentValidation;
using Order.Model;

namespace Order.Validation
{
    public class OrderCartValidation:AbstractValidator<OrderCart>
    {
        public OrderCartValidation()
        { 
            RuleFor(ocv => ocv.product_id).NotEmpty().NotNull().WithMessage("enter valid product Id");
            RuleFor(ocv => ocv.product_name).NotEmpty().NotNull().WithMessage("enter a valid name");
            RuleFor(ocv => ocv.Quantity).NotEmpty().NotNull().GreaterThanOrEqualTo(1).WithMessage("you enter an invalid quantity");
        }
    }
}

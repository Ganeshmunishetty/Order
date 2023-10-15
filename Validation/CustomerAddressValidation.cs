using FluentValidation;
using Order.Model;

namespace Order.Validation
{
    public class CustomerAddressValidation:AbstractValidator<CustomerAddress>
    {
        public CustomerAddressValidation() 
        {
            RuleFor(cav => cav.street).NotEmpty().NotNull().WithMessage("Street cannot be null");
            RuleFor(cav => cav.city).NotEmpty().NotNull().MaximumLength(30).WithMessage("city cannot be empty");
            RuleFor(cav => cav.state).NotEmpty().NotNull().MaximumLength(30).WithMessage("state cannot be empty");
            RuleFor(cav => cav.zip_code).NotEmpty().NotNull().Length(6).MinimumLength(6).WithMessage("enter valid zipcode");
        }
    }
}

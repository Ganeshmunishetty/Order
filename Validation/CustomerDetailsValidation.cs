using FluentValidation;
using Order.Model;

namespace Order.Validation
{
    public class CustomerDetailsValidation:AbstractValidator<CustomerOrderDetails>
    {
        public CustomerDetailsValidation() 
        {
            RuleFor(cdv => cdv.customer_name).NotEmpty().NotNull().WithMessage("Name cannot be empty");
            RuleFor(cdv => cdv.email).EmailAddress().NotNull().NotEmpty().WithMessage("Enter a Valid Emailaddress");
        
        }
    }
}

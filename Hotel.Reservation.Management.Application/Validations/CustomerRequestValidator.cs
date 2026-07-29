using FluentValidation;
using Hotel.Reservation.Management.Application.Contracts.Request;

namespace Hotel.Reservation.Management.Application.Validations
{
    public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerRequestValidator()
        {
            RuleFor(r => r.firstName)
                .NotEmpty()
                .WithMessage("Customer first name is required.");

            RuleFor(r => r.lastName)
                .NotEmpty()
                .WithMessage("Customer last name is required.");
 
            RuleFor(r => r.email)
                .NotEmpty()
                .WithMessage("Customer email is required.")
                .EmailAddress()
                .WithMessage("Customer email must be a valid email address.");
        }
    }
}
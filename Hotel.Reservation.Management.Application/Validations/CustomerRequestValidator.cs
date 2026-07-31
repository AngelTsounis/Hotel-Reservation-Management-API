using FluentValidation;
using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Interfaces;

namespace Hotel.Reservation.Management.Application.Validations
{
    public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerRequestValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(r => r.FirstName)
                .NotEmpty()
                .WithMessage("Customer first name is required.");

            RuleFor(r => r.LastName)
                .NotEmpty()
                .WithMessage("Customer last name is required.");
 
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Customer email is required.")
                .EmailAddress()
                .WithMessage("Customer email must be a valid email address.")
                .MustAsync(BeUniqueEmailAsync)
                .WithMessage(r => $"A customer with email '{r.Email}' already exists.");
        }

        private async Task<bool> BeUniqueEmailAsync(string email, CancellationToken cancellationToken)
        {
            var exists = await _customerRepository.ExistsByEmailAsync(email, cancellationToken);

            return !exists;
        }
    }
}
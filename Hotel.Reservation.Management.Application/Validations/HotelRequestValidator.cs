using FluentValidation;
using Hotel.Reservation.Management.Application.Contracts.Request;

namespace Hotel.Reservation.Management.Application.Validations
{
    public class HotelRequestValidator : AbstractValidator<HotelRequest>
    {
        private const int MinimumStars = 1;
        private const int MaximumStars = 5;

        public HotelRequestValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Hotel name is required.");

            RuleFor(r => r.City)
                .NotEmpty()
                .WithMessage("Hotel city is required.");

            RuleFor(r => r.Stars)
                .InclusiveBetween(MinimumStars, MaximumStars)
                    .WithMessage($"Hotel stars must be between {MinimumStars} and {MaximumStars}.");
        }
    }
}
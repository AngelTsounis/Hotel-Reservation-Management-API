using FluentValidation;
using Hotel.Reservation.Management.Application.Contracts.Request;

namespace Hotel.Reservation.Management.Application.Validations
{
    public class ReservationRequestValidator : AbstractValidator<ReservationRequest>
    {
        public ReservationRequestValidator()
        {
            RuleFor(r => r.HotelId)
                .GreaterThan(0)
                .WithMessage("Hotel ID must be a valid identifier.");

            RuleFor(r => r.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer ID must be a valid identifier.");

            RuleFor(r => r.CheckInDate)
                .NotEqual(default(DateTime))
                .WithMessage("Check-in date is required.");

            RuleFor(r => r.CheckOutDate)
                .NotEqual(default(DateTime))
                .WithMessage("Check-out date is required.")
                .GreaterThan(r => r.CheckInDate)
                    .WithMessage("Check-out date must be after the check-in date.");

            RuleFor(r => r.TotalPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total price cannot be negative.");
        }
    }
}
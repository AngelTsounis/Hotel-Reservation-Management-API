using Hotel.Reservation.Management.Domain.Enums;
using Hotel.Reservation.Management.Domain.Exceptions;

namespace Hotel.Reservation.Management.Domain.Model
{
    public class ReservationEntity : BaseEntity
    {
        //Entity's Framework requires a parameterless constructor for materialization, so we provide a private one.
        private ReservationEntity()
        {
        }

        public ReservationEntity(
            long hotelId,
            long customerId,
            DateTime checkInDate,
            DateTime checkOutDate,
            decimal totalPrice)
        {
            if (checkOutDate <= checkInDate)
            {
                throw new BusinessRuleException(
                    "Check-out date must be after the check-in date.");
            }

            if (checkOutDate <= checkInDate)
            {
                throw new BusinessRuleException(
                    "Check-out date must be after the check-in date.");
            }

            if (totalPrice < 0)
            {
                throw new BusinessRuleException("Total price cannot be negative.");
            }

            HotelId = hotelId;
            CustomerId = customerId;
            CheckInDate = checkInDate.Date;
            CheckOutDate = checkOutDate.Date;
            TotalPrice = totalPrice;
            Status = ReservationStatus.Active;
        }

        public long HotelId { get; private set; }

        public HotelEntity Hotel { get; private set; } = null!;

        public long CustomerId { get; private set; }

        public CustomerEntity Customer { get; private set; } = null!;

        public DateTime CheckInDate { get; private set; }

        public DateTime CheckOutDate { get; private set; }

        public decimal TotalPrice { get; private set; }

        public ReservationStatus Status { get; private set; } = ReservationStatus.Active;

        public bool IsActive => Status == ReservationStatus.Active;

        public bool OverlapsWith(DateTime checkIn, DateTime checkOut) =>
            IsActive &&
            CheckInDate < checkOut.Date &&
            checkIn.Date < CheckOutDate;

        public void Cancel()
        {
            if (Status == ReservationStatus.Cancelled)
            {
                throw new BusinessRuleException(
                    "Reservation has already been cancelled.");
            }

            Status = ReservationStatus.Cancelled;
        }
    }
}

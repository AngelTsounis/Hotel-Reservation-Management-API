using System.Net.Mail;
using Hotel.Reservation.Management.Domain.Exceptions;

namespace Hotel.Reservation.Management.Domain.Model
{
    public class CustomerEntity : BaseEntity
    {
        private CustomerEntity()
        {
        }

        public CustomerEntity(string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new BusinessRuleException("First name is required.");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new BusinessRuleException("Last name is required.");
            }

            if (!MailAddress.TryCreate(email, out _))
            {
                throw new BusinessRuleException("A valid email address is required.");
            }

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim().ToLowerInvariant();
        }

        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        public ICollection<ReservationEntity> Reservations { get; private set; } = [];
    }
}
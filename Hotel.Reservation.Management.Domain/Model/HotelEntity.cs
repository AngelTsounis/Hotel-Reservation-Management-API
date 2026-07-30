using Hotel.Reservation.Management.Domain.Exceptions;

namespace Hotel.Reservation.Management.Domain.Model
{
    public class HotelEntity : BaseEntity
    {
        private const int MinimumStars = 1;
        private const int MaximumStars = 5;

        private HotelEntity()
        {
        }

        public HotelEntity(string name, string city, int stars)
        {
            Update(name, city, stars);
        }

        public string Name { get; private set; } = string.Empty;

        public string City { get; private set; } = string.Empty;

        public int Stars { get; private set; }

        public void Update(string name, string city, int stars)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleException("Hotel name is required.");
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                throw new BusinessRuleException("Hotel city is required.");
            }

            if (stars < MinimumStars || stars > MaximumStars)
            {
                throw new BusinessRuleException(
                    $"Hotel stars must be between {MinimumStars} and {MaximumStars}.");
            }

            Name = name.Trim();
            City = city.Trim();
            Stars = stars;
        }
    }
}
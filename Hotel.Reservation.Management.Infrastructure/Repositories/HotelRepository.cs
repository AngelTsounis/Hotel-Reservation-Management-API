using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Domain.Model;
using Hotel.Reservation.Management.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Reservation.Management.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _dbContext;

        public HotelRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<HotelEntity> CreateHotelEntryAsync(HotelEntity hotel, CancellationToken cancellationToken = default)
        {
            await _dbContext.Hotels.AddAsync(hotel, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return hotel;
        }
    }
}

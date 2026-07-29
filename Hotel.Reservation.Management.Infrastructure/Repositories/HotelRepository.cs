using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Domain.Model;
using Hotel.Reservation.Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Reservation.Management.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _dbContext;

        public HotelRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<HotelEntity> CreateAsync(HotelEntity hotel, CancellationToken cancellationToken = default)
        {
            await _dbContext.Hotels.AddAsync(hotel, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return hotel;
        }

        public async Task<List<HotelEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Hotels
                .AsNoTracking()
                .OrderBy(h => h.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<HotelEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Hotels
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<HotelEntity> UpdateAsync(HotelEntity hotel, CancellationToken cancellationToken = default)
        {
            _dbContext.Hotels.Update(hotel);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return hotel;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var rowsAffected = await _dbContext.Hotels
                .Where(h => h.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return rowsAffected > 0;
        }
    }
}

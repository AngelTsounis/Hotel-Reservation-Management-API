using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Domain.Enums;
using Hotel.Reservation.Management.Domain.Model;
using Hotel.Reservation.Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Reservation.Management.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _dbContext;

        public ReservationRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<ReservationEntity> CreateAsync(ReservationEntity reservation, CancellationToken cancellationToken)
        {
            await _dbContext.Reservations.AddAsync(reservation, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return reservation;
            
        }

        public async Task<IReadOnlyList<ReservationEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Reservations
                .AsNoTracking()
                .OrderBy(r => r.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<ReservationEntity?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await _dbContext.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken); 
        }

        public async Task<IReadOnlyList<ReservationEntity>> GetStatusActiveByCustomerAsync(long customerId, CancellationToken cancellationToken)
        {
            return await _dbContext.Reservations
                .AsNoTracking()
                .Where(r => r.CustomerId == customerId && r.Status == ReservationStatus.Active)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> CancelAsync(long id, CancellationToken cancellationToken)
        {
            var reservation = await _dbContext.Reservations
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (reservation is null)
            {
                return false;
            }

            reservation.Cancel();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

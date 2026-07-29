using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Domain.Model;
using Hotel.Reservation.Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Reservation.Management.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<CustomerEntity> CreateAsync(CustomerEntity customer, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(customer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return customer;
        }

        public async Task<IReadOnlyList<CustomerEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<CustomerEntity?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}

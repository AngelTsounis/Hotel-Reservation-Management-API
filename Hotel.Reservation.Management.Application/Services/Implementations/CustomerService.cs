using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Application.Mapping;
using Hotel.Reservation.Management.Application.Services.Interfaces;
using Hotel.Reservation.Management.Domain.Exceptions;
using Hotel.Reservation.Management.Domain.Model;

namespace Hotel.Reservation.Management.Application.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> CreateAsync(CustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = new CustomerEntity(request.firstName, request.lastName, request.email);

        var customerCreated = await _customerRepository.CreateAsync(customer, cancellationToken); 
        
        return customerCreated.ToCustomerResponse();
    }

    public async Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(cancellationToken);   

        return customers.Select(c => c.ToCustomerResponse()).ToList();
    }

    public async Task<CustomerResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            throw new NotFoundException($"Customer with ID {id} not found.");
        }

        return customer.ToCustomerResponse();
    }
}

using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Domain.Model;
 

namespace Hotel.Reservation.Management.Application.Mapping;

public static class CustomerMapping
{
    public static CustomerResponse ToCustomerResponse(this CustomerEntity entity)
    {
        return new CustomerResponse
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email
        };
    }
}

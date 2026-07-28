using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Reservation.Management.Application.Contracts.Response;

public class HotelResponse
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? City { get; set; }

    public int Stars { get; set; }
}

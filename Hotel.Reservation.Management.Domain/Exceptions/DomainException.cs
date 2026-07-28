using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Reservation.Management.Domain.Exceptions;

public abstract class DomainException(string message) : Exception(message);

public class NotFoundException(string message) : DomainException(message);

public class ConflictException(string message) : DomainException(message);

public class BusinessRuleException(string message) : DomainException(message);

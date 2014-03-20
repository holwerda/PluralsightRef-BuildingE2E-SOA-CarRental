using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarRental.Business.Entities;
using CarRental.Data.Contracts.DTOs;

using Core.Common.Contracts;

namespace CarRental.Data.Contracts.Repository_Interface
{
    public interface IReservationRepository : IDataRepository<Reservation>
    {
        IEnumerable<Reservation> GetReservationByPickupDate(DateTime pickupDate);

        IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo();

        IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId);
    }
}

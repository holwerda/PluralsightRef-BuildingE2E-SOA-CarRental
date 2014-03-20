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
    public interface IRentalRepository : IDataRepository<Rental>
    {
        IEnumerable<Rental> GetRentalHistoryByCar(int carId);

        Rental GetCurrentRentalByCar(int carId);

        IEnumerable<Rental> GetCurrentlyRentedCars();

        IEnumerable<Rental> GetRentalHistoryByAccount(int accountId);

        IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo();
    }
}

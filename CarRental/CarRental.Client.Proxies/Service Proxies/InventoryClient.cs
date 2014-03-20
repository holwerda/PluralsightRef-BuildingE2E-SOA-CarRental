using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using CarRental.Client.Contracts.Service_Contracts;
using CarRental.Client.Entities;

namespace CarRental.Client.Proxies
{
    [Export(typeof(IInventoryService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InventoryClient  : ClientBase<IInventoryService>, IInventoryService
    {
        public Car GetCar(int carId)
        {
            return Channel.GetCar(carId);
        }

        public Car[] GetAllCars()
        {
            return Channel.GetAllCars();
        }

        public Car UpdateCar(Car car)
        {
            return Channel.UpdateCar(car);
        }

        public void DeleteCar(int carId)
        {
            Channel.DeleteCar(carId);
        }

        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return Channel.GetAvailableCars(pickupDate, returnDate);
        }

        public Task<Car> GetCarAsync(int carId)
        {
            return Channel.GetCarAsync(carId);
        }

        public Task<Car[]> GetAllCarsAsync()
        {
            return Channel.GetAllCarsAsync();
        }

        public Task<Car> UpdateCarAsync(Car car)
        {
            return Channel.UpdateCarAsync(car);
        }

        public Task DeleteCarAsync(int carId)
        {
            return Channel.DeleteCarAsync(carId);
        }

        public Task<Car[]> GetAvailableCarsAsync(DateTime pickupDate, DateTime returnDate)
        {
            return Channel.GetAvailableCarsAsync(pickupDate, returnDate);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using CarRental.Client.Contracts.Data_Contracts;
using CarRental.Client.Entities;
using CarRental.Common;

using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace CarRental.Client.Contracts.Service_Contracts
{
    [ServiceContract]
    public interface IRentalService : IServiceContract
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(CarCurrentlyRentedException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Rental RentCarToCustomerImmediate(string loginEmail, int carId, DateTime dateDueBack);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack);
        
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        void AcceptCarReturn(int carId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        IEnumerable<Rental> GetRentalHistory(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Reservation GetReservation(int reservationId);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(CarCurrentlyRentedException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        void ExecuteRentalFromReservation(int reservationId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void CancelReservation(int reservationId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        CustomerReservationData[] GetCurrentReservations();

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        CustomerReservationData[] GetCustomerReservations(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        Rental GetRental(int rentalId);

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        CustomerRentalData[] GetCurrentRentals();

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        Reservation[] GetDeadReservations();

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        bool IsCarCurrentlyRented(int carId);
        
        [OperationContract]
        Task<Rental> RentCarToCustomerImmediateAsync(string loginEmail, int carId, DateTime dateDueBack);

        [OperationContract]
        Task<Rental> RentCarToCustomerAsync(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack);

        [OperationContract]
        Task AcceptCarReturnAsync(int carId);

        [OperationContract]
        Task<IEnumerable<Rental>> GetRentalHistoryAsync(string loginEmail);

        [OperationContract]
        Task<Reservation> GetReservationAsync(int reservationId);

        [OperationContract]
        Task<Reservation> MakeReservationAsync(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate);

        [OperationContract]
        Task ExecuteRentalFromReservationAsync(int reservationId);

        [OperationContract]
        Task CancelReservationAsync(int reservationId);

        [OperationContract]
        Task<CustomerReservationData[]> GetCurrentReservationsAsync();

        [OperationContract]
        Task<CustomerReservationData[]> GetCustomerReservationsAsync(string loginEmail);

        [OperationContract]
        Task<Rental> GetRentalAsync(int rentalId);

        [OperationContract]
        Task<CustomerRentalData[]> GetCurrentRentalsAsync();

        [OperationContract]
        Task<Reservation[]> GetDeadReservationsAsync();

        [OperationContract]
        Task<bool> IsCarCurrentlyRentedAsync(int carId);


    }
}

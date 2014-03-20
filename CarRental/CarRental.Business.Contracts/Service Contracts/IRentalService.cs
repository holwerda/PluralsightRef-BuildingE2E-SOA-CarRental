using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using CarRental.Business.Contracts.Data_Contracts;
using CarRental.Business.Entities;
using CarRental.Common;

using Core.Common.Exceptions;

namespace CarRental.Business.Contracts.Service_Contracts
{
    [ServiceContract]
    public interface IRentalService
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


    }
}

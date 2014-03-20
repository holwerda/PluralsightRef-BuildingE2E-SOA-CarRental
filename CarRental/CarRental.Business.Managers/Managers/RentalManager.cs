using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using CarRental.Business.Common;
using CarRental.Business.Contracts.Data_Contracts;
using CarRental.Business.Contracts.Service_Contracts;
using CarRental.Business.Entities;
using CarRental.Common;
using CarRental.Data.Contracts.DTOs;
using CarRental.Data.Contracts.Repository_Interface;

using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace CarRental.Business.Managers.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class RentalManager : ManagerBase, IRentalService
    {
        public RentalManager()
        {
           
        }

        public RentalManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public RentalManager(IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public RentalManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }

        [Import]
        private IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        private IBusinessEngineFactory _BusinessEngineFactory;

        protected override Account LoadAuthorizationValidationAccount(string loginName)
        {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            Account authAcct = accountRepository.GetByLogin(loginName);
            if (authAcct == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("Cannot find account for login name {0} to use for security trimming.", loginName));
                throw new FaultException<NotFoundException>(ex, ex.Message);

            }
            return authAcct;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Rental RentCarToCustomerImmediate(string loginEmail, int carId, DateTime dateDueBack)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(loginEmail, carId, DateTime.Now, dateDueBack);
                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
              
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(loginEmail, carId, rentalDate, dateDueBack);
                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public void AcceptCarReturn(int carId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                Rental rental = rentalRepository.GetCurrentRentalByCar(carId);

                if (rental == null)
                {
                    CarNotRentedException ex = new CarNotRentedException(string.Format("Car {0} is not currently rented", carId));

                    throw new FaultException<CarNotRentedException>(ex, ex.Message);

                }

                rental.DateReturned = DateTime.Now;

                Rental updatedRentalEntity = rentalRepository.Update(rental);


            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                
                Account account = accountRepository.GetByLogin(loginEmail);

                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found for login '{0}'", loginEmail ));

                    throw new FaultException<NotFoundException>(ex, ex.Message);

                }

                ValidateAuthorization(account);

                IEnumerable<Rental> rentalHistory = rentalRepository.GetRentalHistoryByAccount(account.AccountId);


                return rentalHistory;

            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
        public Reservation GetReservation(int reservationId)
        {
            return ExecuteFaultHandledOperation(() =>
                {
                    IAccountRepository accountRepository =_DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                    IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                    Reservation reservation = reservationRepository.Get(reservationId);

                    if (reservation == null)
                    {
                        NotFoundException ex = new NotFoundException(string.Format("No reservation found for ID '{0}'", reservationId));

                        throw new FaultException<NotFoundException>(ex, ex.Message);
                    }

                    ValidateAuthorization(reservation);

                    return reservation;
                });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
          
            Account account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("No account found for login '{0}'", loginEmail));

                throw new FaultException<NotFoundException>(ex, ex.Message);
            }

            ValidateAuthorization(account);

            Reservation reservation = new Reservation()
                {
                    AccountId = account.AccountId,
                    CarId = carId,
                    RentalDate = rentalDate,
                    ReturnDate = returnDate
                };

            Reservation savedEntity = reservationRepository.Add(reservation);

            return savedEntity;
        }


        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public void ExecuteRentalFromReservation(int reservationId)
        {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IReservationRepository reservationRepository =_DataRepositoryFactory.GetDataRepository<IReservationRepository>();
            ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

            Reservation reservation = reservationRepository.Get(reservationId);
            if (reservation == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("Reservation {0} is not found", reservationId));

                throw new FaultException<NotFoundException>(ex, ex.Message);
            }

            Account account = accountRepository.Get(reservation.AccountId);
            if (account == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("No account found for account ID '{0}'", reservation.AccountId));

                throw new FaultException<NotFoundException>(ex, ex.Message);
            }

            try
            {
                Rental rental = carRentalEngine.RentCarToCustomer(account.LoginEmail, reservation.CarId, reservation.RentalDate, reservation.ReturnDate);
            }
            catch (UnableToRentForDateException ex)
            {
                throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
            }
            catch (CarCurrentlyRentedException ex)
            {
                throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
            }
            catch (NotFoundException ex)
            {
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }
            
            reservationRepository.Remove(reservation);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
        public void CancelReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                
                Reservation reservation = reservationRepository.Get(reservationId);

                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No reservation found for ID '{0}'", reservationId));

                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(reservation);

                reservationRepository.Remove(reservationId);
            });
        }

         [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
         public CustomerReservationData[] GetCurrentReservations()
         {
             return ExecuteFaultHandledOperation(() =>
             {
                 IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                 
                 List<CustomerReservationData> reservationData = new List<CustomerReservationData>();

                 IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCurrentCustomerReservationInfo();

                 foreach (var reservationInfo in reservationInfoSet)
                 {
                     reservationData.Add(new CustomerReservationData()
                     {
                         ReservationId = reservationInfo.Reservation.ReservationId,
                         Car = reservationInfo.Car.Color + " " + reservationInfo.Car.Year + " " + reservationInfo.Car.Description,
                         CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                         RentalDate = reservationInfo.Reservation.RentalDate,
                         ReturnDate = reservationInfo.Reservation.ReturnDate
                     });
                 }

                 return reservationData.ToArray();
             });
         }

         [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
         [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
         public CustomerReservationData[] GetCustomerReservations(string loginEmail)
         {
             return ExecuteFaultHandledOperation(() =>
                 {
                     IAccountRepository accountRepository =_DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                     IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                     Account account = accountRepository.GetByLogin(loginEmail);
                     if (account == null)
                     {
                         NotFoundException ex = new NotFoundException(string.Format("No account found for login '{0}'", loginEmail));

                         throw new FaultException<NotFoundException>(ex, ex.Message);
                     }

                     ValidateAuthorization(account);

                     List<CustomerReservationData> reservationData = new List<CustomerReservationData>();

                     IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCustomerOpenReservationInfo(account.AccountId);

                     foreach (var reservationInfo in reservationInfoSet)
                     {
                         reservationData.Add(new CustomerReservationData()
                         {
                             ReservationId = reservationInfo.Reservation.ReservationId,
                             Car = reservationInfo.Car.Color + " " + reservationInfo.Car.Year + " " + reservationInfo.Car.Description,
                             CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                             RentalDate = reservationInfo.Reservation.RentalDate,
                             ReturnDate = reservationInfo.Reservation.ReturnDate
                         });
                     }

                     return reservationData.ToArray();
             });
         }

         [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
         [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
         public Rental GetRental(int rentalId)
         {
             return ExecuteFaultHandledOperation(
                 () =>
                     {
                         IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                         IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                         Rental rental = rentalRepository.Get(rentalId);

                         if (rental == null)
                         {
                             NotFoundException ex =
                                 new NotFoundException(string.Format("No rental record for id '{0}'", rentalId));

                             throw new FaultException<NotFoundException>(ex, ex.Message);
                         }

                         ValidateAuthorization(rental);

                         return rental;
                     });
         }

         [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
         public CustomerRentalData[] GetCurrentRentals()
         {
             return ExecuteFaultHandledOperation(() =>
             {
                 IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                 List<CustomerRentalData> rentalData = new List<CustomerRentalData>();

                 IEnumerable<CustomerRentalInfo> rentalInfoSet = rentalRepository.GetCurrentCustomerRentalInfo();
                 foreach (var rentalInfo in rentalInfoSet)
                 {
                     rentalData.Add(new CustomerRentalData()
                         {
                             RentalId = rentalInfo.Rental.RentalId,
                             Car = rentalInfo.Car.Color + " " + rentalInfo.Car.Year +  " " + rentalInfo.Car.Description,
                             CustomerName = rentalInfo.Customer.FirstName + " " + rentalInfo.Customer.LastName,
                             DateRented = rentalInfo.Rental.DateRented,
                             ExpectedReturn = rentalInfo.Rental.DateDue
                         });
                 }

                 return rentalData.ToArray();
             });
         }

         [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
         public Reservation[] GetDeadReservations()
         {
             return ExecuteFaultHandledOperation(() =>
             {
                 IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                 IEnumerable<Reservation> reservations =
                     reservationRepository.GetReservationByPickupDate(DateTime.Now.AddDays(-1));

                 return (reservations != null ? reservations.ToArray() : null);
             });
         }

         [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
         public bool IsCarCurrentlyRented(int carId)
         {
             return ExecuteFaultHandledOperation(() =>
                     {
                         ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();
                         return carRentalEngine.IsCarCurrentlyRented(carId);
                     });
         }
    }
}

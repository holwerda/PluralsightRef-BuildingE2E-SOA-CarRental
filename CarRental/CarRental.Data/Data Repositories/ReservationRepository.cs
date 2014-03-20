using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarRental.Business.Entities;
using CarRental.Data.Contracts.DTOs;
using CarRental.Data.Contracts.Repository_Interface;

using Core.Common.Data;
using Core.Common.Extensions;

namespace CarRental.Data.Data_Repositories
{
    [Export(typeof(IReservationRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ReservationRepository : DataRepositoryBase<Reservation>, IReservationRepository
    {
        protected override Reservation AddEntity(CarRentalContext entityContext, Reservation entity)
        {
            return entityContext.ReservationSet.Add(entity);
        }

        protected override Reservation UpdateEntity(CarRentalContext entityContext, Reservation entity)
        {
            return (from e in entityContext.ReservationSet where e.ReservationId == entity.ReservationId select e).FirstOrDefault();
        }

        protected override IEnumerable<Reservation> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.ReservationSet select e;
        }

        protected override Reservation GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.ReservationSet where e.ReservationId == id select e);

            var results = query.FirstOrDefault();

            return results;
        }

        public IEnumerable<Reservation> GetReservationByPickupDate(DateTime pickupDate)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from r in entityContext.ReservationSet where r.RentalDate < pickupDate select r;

                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo()
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from r in entityContext.ReservationSet
                            join a in entityContext.AccountSet on r.AccountId equals a.AccountId
                            join c in entityContext.CarSet on r.CarId equals c.CarId
                            select new CustomerReservationInfo() { Customer = a, Car = c, Reservation = r };
                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from r in entityContext.ReservationSet
                            join a in entityContext.AccountSet on r.AccountId equals a.AccountId
                            join c in entityContext.CarSet on r.CarId equals c.CarId
                            where r.AccountId == accountId
                            select new CustomerReservationInfo() { Customer = a, Car = c, Reservation = r };
                return query.ToFullyLoaded();
            }
        }
    }
}

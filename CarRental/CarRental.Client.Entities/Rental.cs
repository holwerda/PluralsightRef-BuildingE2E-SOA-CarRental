using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Rental : ObjectBase
    {
        private int rentalId;

        private int accountId;

        private int carId;

        private DateTime dateRented;

        private DateTime dateDue;

        private DateTime? dateReturned;

        public int RentalId
        {
            get
            {
                return rentalId;
            }
            set
            {
                if (rentalId != value)
                {
                    rentalId = value;
                    this.OnPropertyChanged(() => this.RentalId);
                }
            }
        }

        public int AccountId
        {
            get
            {
                return accountId;
            }
            set
            {
                if (accountId != value)
                {
                    accountId = value;
                    this.OnPropertyChanged(() => this.AccountId);
                }
                
            }
        }

        public int CarId
        {
            get
            {
                return carId;
            }
            set
            {
                if (carId != value)
                {
                    carId = value;
                    this.OnPropertyChanged(() => this.CarId);
                }
                
            }
        }

        public DateTime DateRented
        {
            get
            {
                return dateRented;
            }
            set
            {

                if (dateRented != value)
                {
                    dateRented = value;
                    this.OnPropertyChanged(() => this.DateRented);
                }
                
            }
        }

        public DateTime DateDue
        {
            get
            {
                return dateDue;
            }
            set
            {
                if (dateDue != value)
                {
                    dateDue = value;
                    this.OnPropertyChanged(() => this.DateDue);
                }
            }
        }

        public DateTime? DateReturned
        {
            get
            {
                return dateReturned;
            }
            set
            {
                if (dateReturned != value)
                {
                    dateReturned = value;
                    this.OnPropertyChanged(() => this.DateReturned);
                }
            }
        }
    }
}

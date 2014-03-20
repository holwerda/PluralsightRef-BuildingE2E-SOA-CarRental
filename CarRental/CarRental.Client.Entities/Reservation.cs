using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Reservation : ObjectBase
    {
        private int reservationId;

        private int accountId;

        private int carId;

        private DateTime rentalDate;

        private DateTime returnDate;

        public int ReservationId
        {
            get
            {
                return reservationId;
            }
            set
            {
                if (reservationId != value)
                {
                    reservationId = value;
                    this.OnPropertyChanged(() => this.ReservationId);
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

        public DateTime RentalDate
        {
            get
            {
                return rentalDate;
            }
            set
            {
                if (rentalDate != value)
                {
                    rentalDate = value;
                    this.OnPropertyChanged(() => this.RentalDate);
                }
            }
        }

        public DateTime ReturnDate
        {
            get
            {
                return returnDate;
            }
            set
            {
                if (returnDate != value)
                {
                    returnDate = value;
                    this.OnPropertyChanged(() => this.ReturnDate);
                }
            }
        }
    }
}

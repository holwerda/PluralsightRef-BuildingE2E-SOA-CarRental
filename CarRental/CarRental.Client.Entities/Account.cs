using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Account : ObjectBase
    {
        private int accountId;

        private string loginEmail;

        private string firstName;

        private string lastName;

        private string address;

        private string city;

        private string state;

        private string zipCode;

        private string creditCard;

        private string expDate;

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

        public string LoginEmail
        {
            get
            {
                return loginEmail;
            }
            set
            {
                if (loginEmail != value)
                {
                    loginEmail = value;
                    this.OnPropertyChanged(() => this.LoginEmail);
                }
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    this.OnPropertyChanged(() => this.FirstName);
                }
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    this.OnPropertyChanged(() => this.LastName);
                }
            }
        }

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                if (address != value)
                {
                    address = value;
                    this.OnPropertyChanged(() => this.Address);
                }
            }
        }

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                if (city != value)
                {
                    city = value;
                    this.OnPropertyChanged(() => this.City);
                }
            }
        }

        public string State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value)
                {
                    state = value;
                    this.OnPropertyChanged(() => this.State);
                }
            }
        }

        public string ZipCode
        {
            get
            {
                return zipCode;
            }
            set
            {
                if (zipCode != value)
                {
                    zipCode = value;
                    this.OnPropertyChanged(() => this.ZipCode);
                }
            }
        }

        public string CreditCard
        {
            get
            {
                return creditCard;
            }
            set
            {
                if (creditCard != value)
                {
                    creditCard = value;
                    this.OnPropertyChanged(() => this.CreditCard);
                }
            }
        }

        public string ExpDate
        {
            get
            {
                return expDate;
            }
            set
            {
                if (expDate != value)
                {
                    expDate = value;
                    this.OnPropertyChanged(() => this.ExpDate);
                }
            }
        }
    }
}

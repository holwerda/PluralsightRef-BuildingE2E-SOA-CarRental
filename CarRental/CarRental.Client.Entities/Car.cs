using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Core;

using FluentValidation;

namespace CarRental.Client.Entities
{
    public class Car : ObjectBase
    {
        private int _CarId;

        private string _Description;

        private string _Color;

        private int _Year;

        private decimal _RentalPrice;

        private bool _CurrentlyRented;

        public int CarId
        {
            get
            {
                return _CarId;
            }
            set
            {
                if (_CarId != value)
                {
                    _CarId = value;

                    this.OnPropertyChanged(() => this.CarId);
                }
                
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;

                    this.OnPropertyChanged(() => this.Description);
                }
            }
        }

        public string Color
        {
            get
            {
                return _Color;
            }
            set
            {
                if (_Color != value)
                {
                    _Color = value;

                    this.OnPropertyChanged(() => this.Color);
                }
            }
        }

        public int Year
        {   
            get
            {
                return _Year;
            }
            set
            {
                if (_Year != value)
                {
                    _Year = value;

                    this.OnPropertyChanged(() => this.Year);
                }
            }
        }

        public decimal RentalPrice
        {
            get
            {
                return _RentalPrice;
            }
            set
            {
                if (_RentalPrice != value)
                {
                    _RentalPrice = value;

                    this.OnPropertyChanged(() => this.RentalPrice);
                }
            }
        }

        public bool CurrentlyRented
        {
            get
            {
                return _CurrentlyRented;
            }
            set
            {
                if (_CurrentlyRented != value)
                {
                    _CurrentlyRented = value;

                    this.OnPropertyChanged(() => this.CurrentlyRented);
                }
            }
        }

        class CarValidator : AbstractValidator<Car>
        {
            public CarValidator()
            {
                RuleFor(obj => obj.Description).NotEmpty();
                RuleFor(obj => obj.Color).NotEmpty();
                RuleFor(obj => obj.RentalPrice).GreaterThan(0);
                RuleFor(obj => obj.Year).GreaterThan(2000).LessThanOrEqualTo(DateTime.Now.Year);
            }
        }

        protected override IValidator GetValidator()
        {
            return new CarValidator();
        }
    }
}

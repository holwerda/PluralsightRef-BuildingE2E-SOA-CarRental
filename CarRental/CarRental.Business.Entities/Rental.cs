using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Contracts;
using Core.Common.Core;

namespace CarRental.Business.Entities
{
    [DataContract]
    public class Rental : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {

        [DataMember]
        public int RentalId { get; set; }

        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public int CarId { get; set; }

        [DataMember]
        public DateTime DateRented { get; set; }

        [DataMember]
        public DateTime DateDue { get; set; }

        [DataMember]
        public DateTime? DateReturned { get; set; }

        public int EntityId
        {
            get
            {
                return RentalId;
            }
            set
            {
                RentalId = value;
            }
        }

        public int OwnerAccountId
        {
            get
            {
                return AccountId;
            }
            set
            {
                AccountId = value;
            }
        }
    }
}

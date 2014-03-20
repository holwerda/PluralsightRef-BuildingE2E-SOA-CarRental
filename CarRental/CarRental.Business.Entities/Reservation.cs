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
    public class Reservation : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int ReservationId { get; set; }

        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public int CarId { get; set; }

        [DataMember]
        public DateTime RentalDate { get; set; }

        [DataMember]
        public DateTime ReturnDate { get; set; }


        public int EntityId
        {
            get
            {
                return ReservationId;
            }
            set
            {
                ReservationId = value;
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

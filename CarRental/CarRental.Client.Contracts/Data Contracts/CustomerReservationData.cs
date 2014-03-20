using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Core.Common.ServiceModel;

namespace CarRental.Client.Contracts.Data_Contracts
{
    public class CustomerReservationData : DataContractBase
    {
        public int ReservationId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string Car { get; set; }

        [DataMember]
        public DateTime RentalDate { get; set; }

        [DataMember]
        public DateTime ReturnDate { get; set; }
    }
}

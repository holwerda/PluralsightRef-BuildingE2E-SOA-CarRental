using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common
{
    public class CarCurrentlyRentedException : ApplicationException
    {
        public CarCurrentlyRentedException(string message) 
            : base(message)
        {
            
        }

         public CarCurrentlyRentedException(string message, Exception exception) 
            : base(message,exception)
        {
            
        }
    }
}

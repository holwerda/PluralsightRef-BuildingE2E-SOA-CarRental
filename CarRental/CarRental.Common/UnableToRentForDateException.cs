using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common
{
    public class UnableToRentForDateException : ApplicationException
    {
        public UnableToRentForDateException(string message) 
            : base(message)
        {
            
        }

        public UnableToRentForDateException(string message, Exception exception) 
            : base(message,exception)
        {
            
        }
    }
}

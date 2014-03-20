using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using CarRental.Business.Entities;
using CarRental.Common;

using Core.Common.Exceptions;

namespace CarRental.Business.Contracts.Service_Contracts
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Account GetCustomerAccountInfo(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        void UpdateCustomerAccountInfo(Account account);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using CarRental.Client.Entities;
using CarRental.Common;

using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace CarRental.Client.Contracts.Service_Contracts
{
    [ServiceContract]
    public interface IAccountService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Account GetCustomerAccountInfo(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        void UpdateCustomerAccountInfo(Account account);

        [OperationContract]
        Task<Account> GetCustomerAccountInfoAsync(string loginEmail);

        [OperationContract]
        Task UpdateCustomerAccountInfoAsync(Account account);
    }
}

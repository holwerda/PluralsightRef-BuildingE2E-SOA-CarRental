using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using CarRental.Client.Contracts.Service_Contracts;
using CarRental.Client.Entities;

namespace CarRental.Client.Proxies
{
    [Export(typeof(IAccountService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountClient : ClientBase<IAccountService>, IAccountService
    {
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return Channel.GetCustomerAccountInfo(loginEmail);
        }

        public void UpdateCustomerAccountInfo(Account account)
        {
            Channel.UpdateCustomerAccountInfo(account);
        }

        public Task<Account> GetCustomerAccountInfoAsync(string loginEmail)
        {
            return Channel.GetCustomerAccountInfoAsync(loginEmail);
        }

        public Task UpdateCustomerAccountInfoAsync(Account account)
        {
            return Channel.UpdateCustomerAccountInfoAsync(account);
        }
    }
}

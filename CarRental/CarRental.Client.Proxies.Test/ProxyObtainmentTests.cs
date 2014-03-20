using System;

using CarRental.Client.Bootstrapper;
using CarRental.Client.Contracts.Service_Contracts;

using Core.Common.Contracts;
using Core.Common.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRental.Client.Proxies.Test
{
    [TestClass]
    public class ProxyObtainmentTests
    {
        [TestMethod]
        public void test_inventory_client_connection()
        {
            InventoryClient proxy = new InventoryClient();

            proxy.Open();
        }

        [TestMethod]
        public void test_account_client_connection()
        {
            AccountClient proxy = new AccountClient();

            proxy.Open();
        }

        [TestMethod]
        public void test_rental_client_connection()
        {
            RentalClient proxy = new RentalClient();

            proxy.Open();
        }
        
    }
}

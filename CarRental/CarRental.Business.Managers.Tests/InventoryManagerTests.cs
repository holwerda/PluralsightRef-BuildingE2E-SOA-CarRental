﻿using System;
using System.Security.Principal;
using System.Threading;

using CarRental.Business.Entities;
using CarRental.Business.Managers.Managers;
using CarRental.Data.Contracts.Repository_Interface;

using Core.Common.Contracts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace CarRental.Business.Managers.Tests
{
    [TestClass]
    public class InventoryManagerTests
    {
        [TestInitialize]
        public void Intitialize()
        {
            GenericPrincipal principal = new GenericPrincipal(
                new GenericIdentity("Miguel"), new string[]{"CarRentalAdmin"});

            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void UpdateCar_add_new()
        {
            Car newCar = new Car();
            Car addedCar = new Car(){CarId = 1};

            Mock<IDataRepositoryFactory> mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(obj => obj.GetDataRepository<ICarRepository>().Add(newCar)).Returns(addedCar);

            InventoryManager manager = new InventoryManager(mockRepositoryFactory.Object);

            Car results = manager.UpdateCar(newCar);

            Assert.IsTrue(results == addedCar);
        }

        [TestMethod]
        public void UpdateCar_update_existing()
        {
            Car existingCar = new Car() { CarId = 1 };
            Car updatedCar = new Car() { CarId = 1 };

            Mock<IDataRepositoryFactory> mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(obj => obj.GetDataRepository<ICarRepository>().Update(existingCar)).Returns(updatedCar);

            InventoryManager manager = new InventoryManager(mockRepositoryFactory.Object);

            Car results = manager.UpdateCar(existingCar);

            Assert.IsTrue(results == updatedCar);
        }
    }
}

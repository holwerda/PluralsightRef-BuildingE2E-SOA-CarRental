using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CarRental.Business.Bootstrapper;
using CarRental.Business.Entities;
using CarRental.Data.Contracts.Repository_Interface;

using Core.Common.Contracts;
using Core.Common.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace CarRental.Data.Tests
{
    [TestClass]
    public class DataLayerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [TestMethod]
        public void test_repository_usage()
        {
            RepositoryTestClass repositoryTest = new RepositoryTestClass();

            IEnumerable<Car> cars = repositoryTest.GetCars();

            Assert.IsTrue(cars != null);
        }

        [TestMethod]
        public void test_repository_factory_usage()
        {
            RepositoryFactoryTestClass repositoryTest = new RepositoryFactoryTestClass();

            IEnumerable<Car> cars = repositoryTest.GetCars();

            Assert.IsTrue(cars != null);
        }

        [TestMethod]
        public void test_repository_mockup()
        {
           List<Car> cars = new List<Car>()
               {
                   new Car(){CarId = 1, Description = "Mustang"},
                   new Car(){CarId = 2, Description = "Corvette"}
               };

            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj => obj.Get()).Returns(cars);

            RepositoryTestClass repositoryTest = new RepositoryTestClass(mockCarRepository.Object);

            IEnumerable<Car> ret = repositoryTest.GetCars();

            Assert.IsTrue(ret == cars);
         
        }

        [TestMethod]
        public void test_repository_factory_mockup()
        {
            List<Car> cars = new List<Car>()
               {
                   new Car(){CarId = 1, Description = "Mustang"},
                   new Car(){CarId = 2, Description = "Corvette"}
               };

            Mock<IDataRepositoryFactory> mockDataRepository = new Mock<IDataRepositoryFactory>();
            mockDataRepository.Setup(obj => obj.GetDataRepository<ICarRepository>().Get()).Returns(cars);

            RepositoryFactoryTestClass repositoryTest = new RepositoryFactoryTestClass(mockDataRepository.Object);

            IEnumerable<Car> ret = repositoryTest.GetCars();

            Assert.IsTrue(ret == cars);

        }

        [TestMethod]
        public void test_repository_factory_mockup2()
        {
            List<Car> cars = new List<Car>()
               {
                   new Car(){CarId = 1, Description = "Mustang"},
                   new Car(){CarId = 2, Description = "Corvette"}
               };

            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj => obj.Get()).Returns(cars);

            Mock<IDataRepositoryFactory> mockDataRepository = new Mock<IDataRepositoryFactory>();
            mockDataRepository.Setup(obj => obj.GetDataRepository<ICarRepository>()).Returns(mockCarRepository.Object);

            RepositoryFactoryTestClass repositoryTest = new RepositoryFactoryTestClass(mockDataRepository.Object);

            IEnumerable<Car> ret = repositoryTest.GetCars();

            Assert.IsTrue(ret == cars);

        }
    }

    public class RepositoryTestClass
    {
        public RepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public RepositoryTestClass(ICarRepository carRepository)
        {
            _CarRepository = carRepository;
        }

        [Import]
        ICarRepository _CarRepository;

        public IEnumerable<Car> GetCars()
        {
            IEnumerable<Car> cars = _CarRepository.Get();
            return cars;
        }

      

    }

    public class RepositoryFactoryTestClass
    {
        public RepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public RepositoryFactoryTestClass(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        public IEnumerable<Car> GetCars()
        {
            ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

            IEnumerable<Car> cars = carRepository.Get();
            return cars;
        }



    }
}

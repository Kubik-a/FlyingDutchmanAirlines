using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines_Tests.Stubs.FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer
{
    [TestClass]
    public class BookingServiceTests
    {
        private  Mock<BookingRepository> _mockBookingRepository;
        private  Mock<CustomerRepository> _mockCustomerRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockBookingRepository = new Mock<BookingRepository>();
            _mockCustomerRepository = new Mock<CustomerRepository>();
        }

        [TestMethod]
        public async Task CreateBooking_Success()
        {
            _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 0)).Returns(Task.CompletedTask);
            _mockCustomerRepository
                .Setup(repository => repository.GetCustomerByName("Alex K"))
                .Returns(Task.FromResult(new Customer("Alex K") { CustomerId = 0 }));

            BookingService service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object);

            (bool result, Exception exception) = await service.CreateBooking("Alex K", 0);

            Assert.IsTrue(result);
            Assert.IsNull(exception);
        }

        [TestMethod]
        [DataRow("", 0)]
        [DataRow(null, -1)]
        [DataRow("Galileo Galilei", -1)]
        public async Task CreateBooking_Failure_InvalidInputArguments(string customerName, int flightNumber)
        {
            BookingService service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object);
            (bool result, Exception exception) = await service.CreateBooking(customerName, flightNumber);

            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public async Task CreateBooking_Failure_RepositoryException_ArgumentException()
        {

            _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 1)).Throws(new ArgumentException());

            _mockCustomerRepository
                .Setup(repository => repository.GetCustomerByName("Galileo Galilei"))
                .Returns(Task.FromResult(new Customer("Galileo Galilei") { CustomerId = 0 }));

            BookingService service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object);
            (bool result, Exception exception) = await service.CreateBooking("Galileo Galilei", 1);

            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));

   
        }

        [TestMethod]
        public async Task CreateBooking_Failure_RepositoryException_CouldNotAddBookingToDatabase()
        {

            _mockBookingRepository.Setup(repository => repository.CreateBooking(1, 2)).Throws(new CouldNotAddBookingToDatabaseException());

            _mockCustomerRepository
                .Setup(repository => repository.GetCustomerByName("Eise Eisinga"))
                .Returns(Task.FromResult(new Customer("Eise Eisinga") { CustomerId = 1 }));

            BookingService service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object);

            (bool result, Exception exception) = await service.CreateBooking("Eise Eisinga", 2);

            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }
    }
}

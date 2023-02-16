using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class CustomerRepositoryTests
    {

        private FlyingDutchmanAirlinesContext _context;
        private CustomerRepository _repository;
        [TestInitialize]
        public async Task Init()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;
            _context = new FlyingDutchmanAirlinesContext(dbContextOptions);

            Customer testCustomer = new Customer("Elon Musk");
            _context.Customers.Add(testCustomer);
            await _context.SaveChangesAsync();


            _repository = new CustomerRepository(_context);
            Assert.IsNotNull(_repository);

        }

        [TestMethod]

        public async Task CreateCustomer_Success()
        {
            CustomerRepository repository = new CustomerRepository(_context);
            Assert.IsNotNull(repository);
            bool result = await repository.CreateCustomer("Alex");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CreateCustomer_Failure_NameIsEmptyString()
        {

            bool result = await _repository.CreateCustomer(string.Empty);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow('#')]
        [DataRow('$')]
        [DataRow('%')]
        [DataRow('&')]
        [DataRow('*')]
        public async Task CreateCustomer_Failure_NameContainsInvalidCharacters(char invalidCharacter)
        {

            bool result = await _repository.CreateCustomer("Alex K" + invalidCharacter);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public async Task CreateCustomer_Failure_DatabaseAccessError()
        {
            CustomerRepository _repo = new CustomerRepository(null);
            bool result = await _repo.CreateCustomer("Donald Knuth");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetCustomerByName_Success()
        {
            Customer customer = await _repository.GetCustomerByName("Elon Musk");
            Assert.IsNotNull(customer);
            Customer dbCustomer = _context.Customers.First();
            /*bool customersAreEqual = customer == dbCustomer;
            Assert.IsTrue(customersAreEqual);*/

            // CLR calls the overloaded operator, it's a shortcut
            Assert.AreEqual(dbCustomer, customer);
        }

        [TestMethod]
        [ExpectedException(typeof(CustomerNotFoundException))]
        public async Task GetCustomerByName_Failure_CustomerNotFound()
        {
            await _repository.GetCustomerByName("Unknown");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("#")]
        [DataRow("$")]
        [DataRow("%")]
        [DataRow("&")]
        [DataRow("*")]
        [ExpectedException(typeof(CustomerNotFoundException))]
        public async Task GetCustomerByName_Failure_InvalidName(string name)
        {
            await _repository.GetCustomerByName(name);
        }
    }
}
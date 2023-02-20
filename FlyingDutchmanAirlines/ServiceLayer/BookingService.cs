using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using System;

namespace FlyingDutchmanAirlines.ServiceLayer
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly CustomerRepository _customerRepository;

        public BookingService(BookingRepository bookingRepository, CustomerRepository customerRepository)
        {
            _bookingRepository = bookingRepository;
            _customerRepository = customerRepository;
        }

        public async Task<(bool, Exception)> CreateBooking(string customerName, int flightNumber)
        {
            if (string.IsNullOrEmpty(customerName) || !flightNumber.IsPositive())
            {
                return (false, new ArgumentException());
            }
            try
            {
                Customer customer;
                try
                {
                    customer = await _customerRepository.GetCustomerByName(customerName);
                }
                catch (CustomerNotFoundException)
                {
                    await _customerRepository.CreateCustomer(customerName);
                    return await CreateBooking(customerName, flightNumber);
                }
                await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }

 
    }
}

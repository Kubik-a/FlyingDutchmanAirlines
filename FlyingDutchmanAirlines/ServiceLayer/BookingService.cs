using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace FlyingDutchmanAirlines.ServiceLayer
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly FlightRepository _flightRepository;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public BookingService()
        {
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor should only be used for testing");
            }
        }

        public BookingService(BookingRepository bookingRepository, CustomerRepository customerRepository, FlightRepository flightRepository)
        {
            _bookingRepository = bookingRepository;
            _customerRepository = customerRepository;
            _flightRepository = flightRepository;
        }

        public virtual async Task<(bool, Exception)> CreateBooking(string name, int flightNumber)
        {
            if (string.IsNullOrEmpty(name) || !flightNumber.IsPositive())
            {
                return (false, new ArgumentException());
            }

            try
            {
                Customer customer = await GetCustomerFromDatabase(name) ?? await AddCustomerToDatabase(name);

                if (!await FlightExistsInDatabase(flightNumber))
                {
                    return (false, new CouldNotAddBookingToDatabaseException());
                }

                await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
                return (true, null);
            }
            catch (Exception exception)
            {
                return (false, exception);
            }
        }

        private async Task<bool> FlightExistsInDatabase(int flightNumber)
        {
            try
            {
                return await _flightRepository.GetFlightByFlightNumber(flightNumber) != null;
            }
            catch (FlightNotFoundException)
            {
                return false;
            }
        }

        private async Task<Customer> GetCustomerFromDatabase(string name)
        {
            try
            {
                return await _customerRepository.GetCustomerByName(name);
            }
            catch (CustomerNotFoundException)
            {
                return null;
            }
            catch (Exception exception)
            {
                ExceptionDispatchInfo.Capture(exception.InnerException ?? new Exception()).Throw();
                return null;
            }
        }

        private async Task<Customer> AddCustomerToDatabase(string name)
        {
            await _customerRepository.CreateCustomer(name);
            return await _customerRepository.GetCustomerByName(name);
        }


    }
}

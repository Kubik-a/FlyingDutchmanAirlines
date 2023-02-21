using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class FlightRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;


        [MethodImpl(MethodImplOptions.NoInlining)]
        public FlightRepository()
        {
            //This avoid this constructor to be invoked in FlyingDutchmanAirlines
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor should only be used for testing");
            }
        }

        public FlightRepository(FlyingDutchmanAirlinesContext _context)
        {
            this._context = _context;
        }


        public virtual async Task<Flight> GetFlightByFlightNumber(int flightNumber)
        {
            if (!flightNumber.IsPositive())
            {
                throw new FlightNotFoundException();
            }

            return await _context.Flights.Where(f => f.FlightNumber == flightNumber).FirstOrDefaultAsync() ?? throw new FlightNotFoundException();
        }
    }
}
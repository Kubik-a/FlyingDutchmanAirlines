using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class AirportRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public AirportRepository()
        {
            //This avoid this constructor to be invoked in FlyingDutchmanAirlines
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor should only be used for testing");
            }
        }

        public AirportRepository(FlyingDutchmanAirlinesContext _context)
        {
            this._context = _context;
        }

        public virtual async Task<Airport> GetAirportByID(int airportID)
        {
            if (!airportID.IsPositive())
            {
                Console.WriteLine($"Argument Exception in GetAirportByID! AirportID = {airportID}");
                throw new ArgumentException("Invalid arguments provided");
            }

            return await _context.Airports.Where(a => a.AirportId == airportID).FirstOrDefaultAsync() ?? throw new AirportNotFoundException();

        }
    }
}

using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class FlightRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;

        public FlightRepository(FlyingDutchmanAirlinesContext _context)
        {
            this._context = _context;
        }


        public async Task<Flight> GetFlightByFlightNumber(int flightNumber, int originAirportId, int destinationAirportId)
        {
            if (!originAirportId.IsPositive() || !destinationAirportId.IsPositive()) {
                Console.WriteLine($"Argument Exception in GetFlightByFlightNumber! originAirportId = {originAirportId} : destinationAirportId = {destinationAirportId}");
                throw new ArgumentException("invalid arguments provided");
            }

            if (!flightNumber.IsPositive())
            {
                throw new FlightNotFoundException();
            }

            return await _context.Flights.Where(f => f.FlightNumber == flightNumber).FirstOrDefaultAsync() ?? throw new FlightNotFoundException();
        }
    }
}
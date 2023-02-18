using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines_Tests.Stubs
{
    namespace FlyingDutchmanAirlines_Tests.Stubs
    {
        class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext
        {
            public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options) : base(options) {
                base.Database.EnsureDeleted();
            }
        
            public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                IEnumerable<EntityEntry> pendingChanges = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
                IEnumerable<Booking> bookings = pendingChanges.Select(e => e.Entity).OfType<Booking>();
                if (bookings.Any(b => b.CustomerId != 1))
                {
                    throw new Exception("Database Error!");
                }

                IEnumerable<Airport> airports = pendingChanges.Select(e => e.Entity).OfType<Airport>();
                if (airports.Any(a => a.AirportId == 10))
                {
                    throw new Exception("Database Error!");
                }

                IEnumerable<Flight> flights = pendingChanges.Select(e => e.Entity).OfType<Flight>();
                if (flights.Any(a => a.FlightNumber == 10))
                {
                    throw new Exception("Database Error!");
                }

                await base.SaveChangesAsync(cancellationToken);
                return 1;
            }
        }
    }
}

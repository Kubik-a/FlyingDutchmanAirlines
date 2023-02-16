using System;
using System.Collections.Generic;

namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public partial class Airport
{
    public int AirportId { get; set; }

    public string City { get; set; }

    public string Iata { get; set; }

    public virtual ICollection<Flight> FlightDestinationNavigations { get; } = new List<Flight>();

    public virtual ICollection<Flight> FlightOriginNavigations { get; } = new List<Flight>();
}

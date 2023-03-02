using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer
{

    [TestClass]
    public class BookingControllerTests
    {
        [TestMethod]
        public async Task CreateBooking_Success()
        {
            Mock<BookingService> service = new Mock<BookingService>();
            service.Setup(s => s.CreateBooking("Alexandre Kubik",1)).Returns(Task.FromResult<(bool error, Exception e)>((true,null)));
            BookingController controller = new BookingController(service.Object);
            BookingData bookingdata = new BookingData()
            {
                FirstName = "Alexandre",
                LastName = "Kubik"
            };
            StatusCodeResult result = await controller.CreateBooking(bookingdata,1) as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);
        }


    }
}

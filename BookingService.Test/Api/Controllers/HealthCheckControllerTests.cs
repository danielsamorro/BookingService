using BookingService.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BookingService.Test.Api.Controllers
{
    public class HealthCheckControllerTests
    {
        private readonly HealthCheckController _healthCheckController;

        public HealthCheckControllerTests()
        {
            _healthCheckController = new HealthCheckController();
        }

        [Fact]
        public void Get_Should_Return_OK()
        {
            var result = _healthCheckController.Get();

            Assert.IsType<OkResult>(result);
        }
    }
}

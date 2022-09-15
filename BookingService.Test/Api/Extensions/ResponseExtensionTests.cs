using BookingService.Api.Extensions;
using BookingService.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BookingService.Test.Api.Extensions
{
    public class ResponseExtensionTests
    {
        public ResponseExtensionTests()
        {

        }

        [Fact]
        public void ToActionResult_Should_Return_OkObjectResult()
        {
            var response = new Response("test");

            var result = response.ToActionResult();

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.Message, (result as OkObjectResult).Value);
        }

        [Fact]
        public void ToActionResult_Should_Return_BadRequestObjectResult()
        {
            var response = new BadRequestResponse("test");

            var result = response.ToActionResult();

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.Errors, (result as BadRequestObjectResult).Value);
        }

        [Fact]
        public void ToActionResult_Should_Return_NotFoundObjectResult()
        {
            var response = new NotFoundResponse("test");

            var result = response.ToActionResult();

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(response.Errors, (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public void ToActionResult_Should_Return_StatusCodeResult_500()
        {
            var response = new ErrorResponse("test");

            var result = response.ToActionResult();

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result as StatusCodeResult).StatusCode);
        }
    }
}

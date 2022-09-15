using BookingService.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Api.Extensions
{
    public static class ResponseExtension
    {
        public static IActionResult ToActionResult(this Response response)
        {
            if (response.Errors.Count > 0)
                if (response.GetType() == typeof(BadRequestResponse))
                    return new BadRequestObjectResult(response.Errors);
                else if (response.GetType() == typeof(NotFoundResponse))
                    return new NotFoundObjectResult(response.Errors);
                else if (response.GetType() == typeof(ErrorResponse))
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            
            return new OkObjectResult(response.Message);            
        }
    }
}

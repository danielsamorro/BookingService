namespace BookingService.Domain.Responses
{
    public class ErrorResponse : Response
    {
        public ErrorResponse(params string[] errors) : base(null, errors) { }
    }
}

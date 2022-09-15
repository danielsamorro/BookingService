namespace BookingService.Domain.Responses
{
    public class ErrorResponse : Response
    {
        public ErrorResponse(params string[] errors) : base(null, errors) { }
        public ErrorResponse(object message, params string[] errors) : base(message, errors) { }
    }
}

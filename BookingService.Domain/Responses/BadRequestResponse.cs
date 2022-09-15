namespace BookingService.Domain.Responses
{
    public class BadRequestResponse : Response
    {
        public BadRequestResponse(params string[] errors) : base(null, errors) { }
        public BadRequestResponse(object message, params string[] errors) : base(message, errors) { }
    }
}

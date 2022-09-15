namespace BookingService.Domain.Responses
{
    public class BadRequestResponse : Response
    {
        public BadRequestResponse(params string[] errors) : base(null, errors) { }
    }
}

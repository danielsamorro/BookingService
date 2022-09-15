namespace BookingService.Domain.Responses
{
    public class NotFoundResponse : Response
    {
        public NotFoundResponse(params string[] errors) : base(null, errors) { }
    }
}

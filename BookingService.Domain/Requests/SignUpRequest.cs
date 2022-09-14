namespace BookingService.Domain.Requests
{
    public class SignUpRequest
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}

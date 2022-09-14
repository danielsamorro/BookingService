using System;
using System.Collections.Generic;
using System.Text;

namespace BookingService.Domain.Responses
{
    public class NotFoundResponse : Response
    {
        public NotFoundResponse(params string[] errors) : base(null, errors) { }
        public NotFoundResponse(object message, params string[] errors) : base(message, errors) { }
    }
}

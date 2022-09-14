using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingService.Domain.Responses
{
    public class Response
    {
        public object Message { get; set; }
        public List<string> Errors { get; set; }

        public Response(object message)
        {
            Message = message;
            Errors = new List<string>();
        }

        public Response(object message, params string[] errors)
        {
            Message = message;
            Errors = errors.ToList();
        }
    }
}

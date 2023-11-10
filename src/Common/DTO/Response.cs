using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace aaa_aspdotnet.src.Common.DTO
{
    public class Response
    {
        public string Status;
        public string Message;
        public object? Result;

        public Response(HttpStatusCode status, string message, object result = null)
        {
            Status = status.ToString();
            Message = message;
            Result = result;
        }
        public Response(HttpStatusCode status, string message)
        {
            Status = status.ToString();
            Message = message;

        }
        //public Response(HttpStatusCode status, string message) : this(status, message, null)
        //{

        //    Result = string.Empty;
        //}


    }
}

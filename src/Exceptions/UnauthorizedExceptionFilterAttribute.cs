using System.Net;
using System.Web.Http.Filters;

namespace aaa_aspdotnet.src.Exceptions
{

    public class UnauthorizedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is UnauthorizedAccessException)
            {
                // You can customize the response message and status code here.
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("Unauthorized access"),
                    ReasonPhrase = "Unauthorized"
                };

                actionExecutedContext.Response = response;
            }
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace aaa_aspdotnet.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
      
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ ở đây (ví dụ: ghi log, trả về mã lỗi phù hợp, ...)
                Console.WriteLine($"Exception: {ex.Message}");

                // Có thể thay đổi mã lỗi HTTP trả về
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                
                // Trả thông điệp lỗi vào body của response
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new {status=StatusCodes.Status500InternalServerError, message = ex.Message }));
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            //var configuration = builder.ApplicationServices.GetService<IConfiguration>();
            //string connectionString = configuration.GetSection("ConnectionStrings")["value"];
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

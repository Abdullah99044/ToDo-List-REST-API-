using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace TodoList.MiddleWare
{
    public class GlobalExceptionHandlingMiddlewarecs : IMiddleware
    {

        //Implment logger

        private readonly  ILogger<GlobalExceptionHandlingMiddlewarecs> _logger;
        public GlobalExceptionHandlingMiddlewarecs(ILogger<GlobalExceptionHandlingMiddlewarecs> logger )
        {
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {

                await next(context);

            }

            catch (Exception e)
            {

                //Log the error

                _logger.LogError(e , e.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails porblem = new ProblemDetails()
                {

                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = "An internal server has occured"
                };

               
                string json = JsonSerializer.Serialize(porblem);


                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);



            }
        }
    }
}

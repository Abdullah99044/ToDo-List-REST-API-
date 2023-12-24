 

namespace TodoList.MiddleWare
{
    public class securityHeadersMiddleWare : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {


            //Put this code instead of "Content-Security-Policy", "default-src 'self'" 
            // and write your client app url  in place of https://todo.com
            // and your api url in place of https://todoApi.com;

            //context.Response.Headers.Add("Content-Security-Policy", 
            //     "default-src 'self'; script-src 'self' https://todo.com https://todoApi.com; " +
            //    "style-src 'self'; img-src 'self' data:; font-src 'self'");

            // Add security headers to the response
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

            await next(context);
        }
    }
}

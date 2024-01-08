 

namespace TodoList.MiddleWare
{
    public class securityHeadersMiddleWare : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            // Add security headers to the response
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

            await next(context);
        }
    }
}

using System.Net;
using Microsoft.AspNetCore.Http;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    internal static class HttpContextExtensions
    {
        public static void ClearAndSetStatusCode(this HttpContext httpContext, HttpStatusCode statusCode)
        {
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)statusCode;
        }
    }
}

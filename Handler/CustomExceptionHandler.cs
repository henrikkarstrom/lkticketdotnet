using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKTicket.Handler
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandler(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InternalException ex)
            {
                try
                {
                    context.Response.StatusCode = (int)ex.HttpStatusCode;
                    await context.Response.WriteAsync(ex.Message);
                    
                }
                catch (Exception ex2)
                {
                    throw ex;
                }

                // Otherwise this handler will
                // re -throw the original exception
                throw;
            }
        }
    }
}

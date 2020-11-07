using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace API.Infrastructure.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (authResult.Succeeded && authResult.Principal.Identity.IsAuthenticated)
            {
                await _next.Invoke(context);
            }
            else if (authResult.Failure != null)
            {
                // Rethrow, let the exception page handle it.
                ExceptionDispatchInfo.Capture(authResult.Failure).Throw();
            }
            else
            {
                await context.ChallengeAsync();
            }
        }
    }
}

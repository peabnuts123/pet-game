using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace PetGame.Web
{
    /// <summary>
    /// Very simple logging middleware. Dunno why I have to write this myself
    /// @TODO is this safe to be logging?
    /// </summary>
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> logger;

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
        {
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string message = "";
            message += $"[{DateTime.Now.ToString("o")}] Request - Trace ID {context.TraceIdentifier}\n";
            message += $"{context.Request.Method} {context.Request.Scheme}://{context.Request.Host}/{context.Request.Path} {context.Request.Protocol}\n";
            foreach (var header in context.Request.Headers)
            {
                message += $"{header.Key}: {CensorHeader(header)}\n";
            }
            message += "\n";
            this.logger.LogInformation(message);

            await next(context);
        }

        private string CensorHeader(KeyValuePair<string, StringValues> header)
        {
            switch (header.Key.ToLowerInvariant())
            {
                case "cookie":
                    return "REDACTED";
                default:
                    return header.Value;
            }
        }
    }
}
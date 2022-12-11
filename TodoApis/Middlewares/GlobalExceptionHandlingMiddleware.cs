using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace TodoApis.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
		private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next(context);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				ProblemDetails problem = new()
				{
					Type = "Error",
					Title = "Error",
					Status = (int)HttpStatusCode.InternalServerError,
					Detail = "Internal server error occured"
				};

				var json = JsonConvert.SerializeObject(problem);

				await context.Response.WriteAsync(json);
				context.Response.ContentType = "application/json";

			}
        }
    }
}

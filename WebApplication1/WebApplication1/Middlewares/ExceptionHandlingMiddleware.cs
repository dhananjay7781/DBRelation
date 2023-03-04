using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;
using WebApplication1.Exceptions;
using WebApplication1.Helpers;

namespace WebApplication1.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            ILogger<ExceptionHandlingMiddleware> logger,
            RequestDelegate next,
            IHostEnvironment env)
        {
            _logger = logger;
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                context.Response.ContentType = "application/json";

                ApiResponse<string> response = new ApiResponse<string>
                {
                    Succeeded = false,
                    Message = ex?.Message
                };

                switch (ex)
                {
                    case ValidationException e:

                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        if (_env.IsDevelopment())
                        {
                            response.Data = ex.StackTrace?.ToString();
                            if (!e.Errors.IsNullOrEmpty())
                            {
                                response.Errors.AddRange(e.Errors);
                            }
                        }
                        else
                        {
                            response.Data = "ValidationException400";
                            if (!e.Errors.IsNullOrEmpty())
                            {
                                response.Errors.AddRange(e.Errors);
                            }
                        }

                        break;

                    case NotFoundException e:

                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        if (_env.IsDevelopment())
                        {
                            response.Data = ex.StackTrace?.ToString();
                            if (!e.Errors.IsNullOrEmpty())
                            {
                                response.Errors.AddRange(e.Errors);
                            }
                        }
                        else
                        {
                            response.Data = "NotFoundException404";
                            if (!e.Errors.IsNullOrEmpty())
                            {
                                response.Errors.AddRange(e.Errors);
                            }
                        }

                        break;

                    case OperationException e:

                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        if (_env.IsDevelopment())
                        {
                            response.Data = ex.StackTrace?.ToString();
                            if (!e.Errors.IsNullOrEmpty())
                            {
                                response.Errors.AddRange(e.Errors);
                            }
                        }
                        else
                        {
                            response.Data = "OperationException500";
                            if (!e.Errors.IsNullOrEmpty())
                            {
                                response.Errors.AddRange(e.Errors);
                            }
                        }

                        break;

                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        if (_env.IsDevelopment())
                        {
                            response.Data = ex.StackTrace?.ToString();
                        }
                        else
                        {
                            response.Data = "InternalServerError500";
                        }
                        break;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}

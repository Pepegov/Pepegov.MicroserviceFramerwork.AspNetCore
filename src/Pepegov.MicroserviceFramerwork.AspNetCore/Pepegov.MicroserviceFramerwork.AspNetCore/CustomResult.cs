using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Pepegov.MicroserviceFramerwork.AspNetCore;

public class CustomResult<T> : IResult
{
    private readonly T _message;
    private readonly int _statusCode;

    public CustomResult(T message, int statusCode)
    {
        _message = message;
        _statusCode = statusCode;
    }
    public Task ExecuteAsync(HttpContext httpContext)
    {
        var serializedMessage = JsonSerializer.Serialize(_message);
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = _statusCode;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(serializedMessage);
        return httpContext.Response.WriteAsync(serializedMessage);
    }
}

public static class CustomResultExtension
{
    public static IResult Custom<T>(this IResultExtensions resultExtensions, T message, int statusCode)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions);

        return new CustomResult<T>(message, statusCode);
    }
}
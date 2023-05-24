using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Pepegov.MicroserviceFramerwork.AspNetCore.Extensions.IResults;

public class CustomResult : IResult
{
    private readonly string _mediaTypeName;
    private readonly int _statusCode;
    
    protected string Message;

    public CustomResult(string message, int statusCode,  string mediaTypeName = MediaTypeNames.Application.Json)
    {
        this.Message = message;
        _statusCode = statusCode;
        _mediaTypeName = mediaTypeName;
    }
    
    public CustomResult(int statusCode, string mediaTypeName = MediaTypeNames.Application.Json)
    {
        _statusCode = statusCode;
        _mediaTypeName = mediaTypeName;
    }
 
    public virtual async Task ExecuteAsync(HttpContext context)
    {
        context.Response.ContentType = _mediaTypeName;
        context.Response.StatusCode = _statusCode;
        context.Response.ContentLength = Encoding.UTF8.GetByteCount(Message);
        await context.Response.WriteAsync(Message);
    }
}
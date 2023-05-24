using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Pepegov.MicroserviceFramerwork.AspNetCore.Extensions.IResults;

public sealed class CustomJsonResult<T> : CustomResult
{
    private readonly T _obj;
    
    public CustomJsonResult(T obj, int statusCode = (int)HttpStatusCode.OK) : base(statusCode, MediaTypeNames.Application.Json)
    {
        ArgumentNullException.ThrowIfNull(obj);
        _obj = obj;
    }

    public override Task ExecuteAsync(HttpContext context)
    {
        Message = JsonSerializer.Serialize(_obj);
        
        return base.ExecuteAsync(context);
    }
}
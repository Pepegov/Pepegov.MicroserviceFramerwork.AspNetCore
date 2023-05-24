using Microsoft.AspNetCore.Http;

namespace Pepegov.MicroserviceFramerwork.AspNetCore.Extensions.IResults;

public static class IResultExtension
{
    public static IResult Custom(this IResultExtensions resultExtensions, string message, int statusCode)
        =>  new CustomResult(message, statusCode);
    
    public static IResult CustomAsJson<T>(this IResultExtensions resultExtensions, T message, int statusCode)
        => new CustomJsonResult<T>(message, statusCode);
    
    public static IResult CustomAsXml<T>(this IResultExtensions resultExtensions, T message, int statusCode)
        => new CustomXmlResult<T>(message, statusCode);
}
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Pepegov.MicroserviceFramerwork.AspNetCore.Infrastructure;

namespace Pepegov.MicroserviceFramerwork.AspNetCore.Extensions.IResults;

public sealed class CustomXmlResult<T> : CustomResult
{
    private readonly T _obj;
    
    public CustomXmlResult(T obj, int statusCode = (int)HttpStatusCode.OK) : base(statusCode, MediaTypeNames.Application.Xml)
    {
        ArgumentNullException.ThrowIfNull(obj);
        _obj = obj;
    }
 
    public override async Task ExecuteAsync(HttpContext context)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("","");
        
        using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
        {
            xmlSerializer.Serialize(sw, _obj, ns);
            Message = sw.ToString();
        }

        await base.ExecuteAsync(context);
    }
}
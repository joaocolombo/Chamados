using System.Net.Http;

namespace MVC.Interfaces
{
    public interface IConsumirApi
    {
        HttpResponseMessage PutMethod(string uri, string uriParametros, string json);
        HttpResponseMessage PostMethod(string uri, string json);
        HttpResponseMessage GetMethod(string uri, string uriParametros);
    }
}
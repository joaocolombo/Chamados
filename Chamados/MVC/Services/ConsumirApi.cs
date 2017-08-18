using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MVC.Interfaces;


namespace MVC.Services
{
    public class ConsumirApi : IConsumirApi

    {
        private string url = "http://10.1.0.4";

        public  HttpResponseMessage GetMethod(string uri, string uriParametros)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url + uri);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                return client.GetAsync(uriParametros).Result;

            };
        }

        public HttpResponseMessage PostMethod(string uri,  string json)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + uri);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                return client.PostAsync(url + uri,new StringContent(json, Encoding.UTF8, "application/json")).Result;
            }
        }

            public HttpResponseMessage PutMethod(string uri, string uriParametros, string json)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url + uri);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                return client.PutAsync(url + uriParametros, new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
            }
        }
    }
}
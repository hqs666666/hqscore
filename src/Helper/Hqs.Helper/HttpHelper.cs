using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Hqs.Helper
{
    public class HttpHelper
    {
        public static object Get(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest("", Method.GET, DataFormat.Json);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject(response.Content);
        }

        public static object Get(string url, Dictionary<string, object> paras)
        {
            var client = new RestClient(url);
            var request = new RestRequest("", Method.GET, DataFormat.Json);
            foreach (var para in paras)
            {
                request.AddParameter(para.Key, para.Value);
            }
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject(response.Content);
        }

        public static object GetWithJwt(string url, Dictionary<string, object> paras, string accessToken)
        {
            var client = new RestClient(url) { Authenticator = new JwtAuthenticator(accessToken) };
            var request = new RestRequest("", Method.GET, DataFormat.Json);
            foreach (var para in paras)
            {
                request.AddParameter(para.Key, para.Value);
            }
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject(response.Content);
        }

        public static object Post(string url, object body)
        {
            var client = new RestClient(url);
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(body);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject(response.Content);
        }

        public static object PostWithJwt(string url, object body, string accessToken)
        {
            var client = new RestClient(url) { Authenticator = new JwtAuthenticator(accessToken) };
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(body);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject(response.Content);
        }

        public static object Post(string url, Dictionary<string, object> param)
        {
            var client = new RestClient(url);
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            request.AddHeader("Accept", "application/json");

            if (param != null)
            {
                foreach (var item in param)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }

            var response = client.Execute(request);
            return JsonConvert.DeserializeObject(response.Content);
        }

        public static object PostWithJwt(string url, Dictionary<string, object> param, string accessToken)
        {
            var client = new RestClient(url) { Authenticator = new JwtAuthenticator(accessToken) };
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            request.AddHeader("Accept", "application/json");

            if (param != null)
            {
                foreach (var item in param)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }

            var response = client.Execute(request);
            return JsonConvert.DeserializeObject(response.Content);
        }
    }
}

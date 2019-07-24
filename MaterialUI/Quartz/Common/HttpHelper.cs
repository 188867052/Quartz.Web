namespace MaterialUI.Job.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Quartz;

    public static class Helper
    {
        internal static string GetString(this IJobExecutionContext context, string key)
        {
            var str = context.JobDetail.JobDataMap.GetString(key);

            return str;
        }
    }

    public class HttpHelper
    {
        public static readonly HttpHelper Instance;

        static HttpHelper()
        {
            Instance = new HttpHelper();
        }

        public static Dictionary<string, HttpClient> Dictionary = new Dictionary<string, HttpClient>();

        private HttpClient GetHttpClient(string url)
        {
            var uri = new Uri(url);
            var key = uri.Scheme + uri.Host;
            if (!Dictionary.Keys.Contains(key))
            {
                Dictionary.Add(key, new HttpClient());
            }

            return Dictionary[key];
        }

        public async Task<HttpResponseMessage> PostAsync(string url, string jsonString, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                jsonString = "{}";
            StringContent content = new StringContent(jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (headers != null && headers.Any())
            {
                // 如果有headers认证等信息，则每个请求实例一个HttpClient
                using (HttpClient http = new HttpClient())
                {
                    foreach (var item in headers)
                    {
                        http.DefaultRequestHeaders.Remove(item.Key);
                        http.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }

                    return await http.PostAsync(new Uri(url), content);
                }
            }
            else
            {
                return await this.GetHttpClient(url).PostAsync(new Uri(url), content);
            }
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T content, Dictionary<string, string> headers = null)
            where T : class
        {
            return await this.PostAsync(url, JsonConvert.SerializeObject(content), headers);
        }

        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            if (headers != null && headers.Any())
            {
                // 如果有headers认证等信息，则每个请求实例一个HttpClient
                using (HttpClient http = new HttpClient())
                {
                    foreach (var item in headers)
                    {
                        http.DefaultRequestHeaders.Remove(item.Key);
                        http.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }

                    return await http.GetAsync(url);
                }
            }
            else
            {
                return await this.GetHttpClient(url).GetAsync(url);
            }
        }

        public async Task<HttpResponseMessage> PutAsync(string url, string jsonString, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                jsonString = "{}";
            }

            StringContent content = new StringContent(jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (headers != null && headers.Any())
            {
                // 如果有headers认证等信息，则每个请求实例一个HttpClient
                using (HttpClient http = new HttpClient())
                {
                    foreach (var item in headers)
                    {
                        http.DefaultRequestHeaders.Remove(item.Key);
                        http.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }

                    return await http.PutAsync(url, content);
                }
            }
            else
            {
                return await this.GetHttpClient(url).PutAsync(url, content);
            }
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T content, Dictionary<string, string> headers = null)
        {
            return await this.PutAsync(url, JsonConvert.SerializeObject(content), headers);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, Dictionary<string, string> headers = null)
        {
            if (headers != null && headers.Any())
            {
                // 如果有headers认证等信息，则每个请求实例一个HttpClient
                using (HttpClient http = new HttpClient())
                {
                    foreach (var item in headers)
                    {
                        http.DefaultRequestHeaders.Remove(item.Key);
                        http.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }

                    return await http.DeleteAsync(url);
                }
            }
            else
            {
                return await this.GetHttpClient(url).DeleteAsync(url);
            }
        }
    }
}

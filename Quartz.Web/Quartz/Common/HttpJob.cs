namespace Quartz.Job.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using Newtonsoft.Json;
    using Quartz;
    using Quartz.Entity;
    using Quartz.Job.Model;
    using Quartz.Logging;
    using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class HttpJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var url = context.JobDetail.JobDataMap.GetString(Constant.RequestUrl);
            url = url?.IndexOf("http") == 0 ? url : "http://" + url;
            var requestParameters = context.GetString(Constant.RequestParameters);
            var logs = context.JobDetail.JobDataMap[Constant.LogList] as List<string> ?? new List<string>();
            var headersString = context.GetString(Constant.Headers);
            var headers = headersString != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(headersString?.Trim()) : null;
            var requestType = (HttpMethod)Enum.Parse(typeof(HttpMethod), context.GetString(Constant.RequestType));
            var loginfo = new LogInfoModel
            {
                Url = url,
                BeginTime = DateTime.Now,
                RequestType = requestType,
                Parameters = requestParameters,
                JobName = $"{context.JobDetail.Key.Group}.{context.JobDetail.Key.Name}",
            };

            try
            {
                var http = HttpHelper.Instance;
                var response = new HttpResponseMessage();
                switch (requestType)
                {
                    case HttpMethod.Get:
                        response = await http.GetAsync(url, headers);
                        break;
                    case HttpMethod.Post:
                        response = await http.PostAsync(url, requestParameters, headers);
                        break;
                    case HttpMethod.Put:
                        response = await http.PutAsync(url, requestParameters, headers);
                        break;
                    case HttpMethod.Delete:
                        response = await http.DeleteAsync(url, headers);
                        break;
                }

                var result = HttpUtility.HtmlEncode(await response.Content.ReadAsStringAsync());

                stopwatch.Stop(); // 停止监视
                double seconds = stopwatch.Elapsed.TotalSeconds;  // 总秒数
                loginfo.EndTime = DateTime.Now;
                loginfo.Seconds = seconds;
                loginfo.Result = result.PadLeft(200);
                if (!response.IsSuccessStatusCode)
                {
                    loginfo.ErrorMsg = result.PadLeft(200);
                    context.JobDetail.JobDataMap[Constant.Exception] = this.SerializeObject(loginfo);
                }
                else
                {
                    using (var dbContext = new QuartzContext())
                    {
                        dbContext.QuartzLog.Add(new QuartzLog()
                        {
                            CreateTime = DateTime.Now,
                            LogLevel = LogLevel.Info,
                            TestData = (HttpMethod)(DateTime.Now.Second % 10),
                            Message = result,
                            Name = context.JobDetail.Key.Name,
                            Group = context.JobDetail.Key.Group,
                        });
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                double seconds = stopwatch.Elapsed.TotalSeconds;
                loginfo.ErrorMsg = ex.Message + ex.StackTrace;
                context.JobDetail.JobDataMap[Constant.Exception] = this.SerializeObject(loginfo);
                loginfo.Seconds = seconds;
            }
            finally
            {
                logs.Add(this.SerializeObject(loginfo));
                context.JobDetail.JobDataMap[Constant.LogList] = logs;
            }
        }

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd hh:mm:ss",
            });
        }
    }
}

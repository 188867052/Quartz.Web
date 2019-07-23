namespace MaterialKit.Controllers
{
    using System.Threading.Tasks;
    using Host.Model;
    using MaterialKit.Job.Entity;
    using Microsoft.AspNetCore.Mvc;
    using MimeKit;
    using Newtonsoft.Json;

    public class SetingController : Controller
    {
        static readonly string FilePath = "File/Mail.txt";
        static readonly string RefreshIntervalPath = "File/RefreshInterval.json";

        static MailEntity mailData = null;

        /// <summary>
        /// 保存Mail信息.
        /// </summary>
        /// <param name="mailEntity">mailEntity.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SaveMailInfo([FromBody]MailEntity mailEntity)
        {
            mailData = mailEntity;
            await System.IO.File.WriteAllTextAsync(FilePath, JsonConvert.SerializeObject(mailEntity));
            return true;
        }

        /// <summary>
        /// 保存刷新间隔.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SaveRefreshInterval([FromBody]RefreshIntervalEntity entity)
        {
            await System.IO.File.WriteAllTextAsync(RefreshIntervalPath, JsonConvert.SerializeObject(entity));
            return true;
        }

        /// <summary>
        /// 获取.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<RefreshIntervalEntity> GetRefreshInterval()
        {
            return JsonConvert.DeserializeObject<RefreshIntervalEntity>(await System.IO.File.ReadAllTextAsync(RefreshIntervalPath));
        }

        /// <summary>
        /// 获取eMail信息.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MailEntity> GetMailInfo()
        {
            if (mailData == null)
            {
                var mail = await System.IO.File.ReadAllTextAsync(FilePath);
                mailData = JsonConvert.DeserializeObject<MailEntity>(mail);
            }

            return mailData;
        }

        /// <summary>
        /// 发送邮件.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SendMail([FromBody]SendMailModel model)
        {
            try
            {
                if (model.MailInfo == null)
                {
                    model.MailInfo = await this.GetMailInfo();
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(model.MailInfo.MailFrom, model.MailInfo.MailFrom));
                foreach (var mailTo in model.MailInfo.MailTo.Replace("；", ";").Replace("，", ";").Replace(",", ";").Split(';'))
                {
                    message.To.Add(new MailboxAddress(mailTo, mailTo));
                }

                message.Subject = string.Format(model.Title);
                message.Body = new TextPart("html")
                {
                    Text = model.Content,
                };
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(model.MailInfo.MailHost, 465, true);
                    client.Authenticate(model.MailInfo.MailFrom, model.MailInfo.MailPwd);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}

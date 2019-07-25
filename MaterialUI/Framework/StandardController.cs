namespace MaterialUI.Framework
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AspNetCore.Extensions;
    using MaterialUI.Controllers;
    using MaterialUI.Entity;
    using MaterialUI.Files;
    using MaterialUI.Html;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Html.Checkbox;
    using MaterialUI.Html.Dialog;
    using MaterialUI.Html.Dialog.Demo;
    using MaterialUI.Html.Inputs;
    using MaterialUI.Html.RadioButtons;
    using MaterialUI.Html.Tables;
    using MaterialUI.Job.Common;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc;
    using Quartz;
    using Quartz.Impl.Matchers;
    using Quartz.Impl.Triggers;

    public class StandardController : Controller
    {
        protected IGetHtml getHtml;
        protected IScheduler scheduler;

        protected StandardController(MaterialUIContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public MaterialUIContext DbContext { get; }

        protected IActionResult HtmlResult(string html)
        {
            return this.Content(html, "text/html", Encoding.UTF8);
        }

        protected IActionResult SearchGrid<TModel, TPostModel>(GridSearchPage<TModel, TPostModel> searchGrid)
        {
            return this.HtmlResult(searchGrid.Render());
        }

        protected IActionResult Page(PageBase page)
        {
            return this.HtmlResult(page.Render());
        }

        protected IActionResult HtmlResult(IHtmlContent html)
        {
            return this.HtmlResult(TagHelper.ToHtml(html));
        }

        protected IActionResult Dialog(DialogBase dialog)
        {
            return this.HtmlResult(TagHelper.ToHtml(dialog.Render()));
        }

        protected IActionResult CommonReplaces(string htmlName)
        {
            var html = this.getHtml.GetContent(htmlName);

            var script = TagHelper.ToHtml(HtmlHelper.GetBodyScript());
            html = html.Replace("{{script}}", script);
            var css = TagHelper.ToHtml(HtmlHelper.GetHeadCss());
            html = html.Replace("{{css}}", css);
            html = html.Replace("{{copyright}}", HtmlHelper.GetCopyRight());
            html = html.Replace("{{navbar}}", Navigation.GetNavbar());
            html = html.Replace("{{footernav}}", HtmlHelper.GetFooter());

            if (htmlName == HtmlFile.ContactUsHtml)
            {
                html = html.Replace("navbar navbar-primary navbar-transparent navbar-absolute", "navbar navbar-inverse navbar-fixed-top");
            }

            if (htmlName == HtmlFile.IndexHtml)
            {
                html = html.Replace("{{ManualRotatingCards}}", new ManualRotatingCard().ToHtml());
                html = html.Replace("{{table}}", new Table().ToHtml());
                html = html.Replace("{{Tooltips}}", Tooltip.Generate());
                html = html.Replace("{{Popovers}}", Popovers.Generate());
                html = html.Replace("{{buttons}}", Button.Generate());
                html = html.Replace("{{pickSizeButtons}}", XSmallButton.Generate());
                html = html.Replace("{{ColoredButton}}", ColoredButton.Generate());
                html = html.Replace("{{Input}}", LabeledInput.Generate());
                html = html.Replace("{{Checkbox}}", Checkbox.Generate());
                html = html.Replace("{{RadioButton}}", RadioButton.Generate());
                html = html.Replace("{{ToggleButton}}", ToggleButton.Generate());
                html = html.Replace("{{Pagination}}", new Pagination().ToHtml(1, 10, 100));
                html = html.Replace("{{Dropdown}}", new Dropdown().ToHtml());
                html = html.Replace("{{Textarea}}", new Textarea().ToHtml());
                html = html.Replace("{{SelectPicker}}", new SingleSelect().ToHtml());
                html = html.Replace("{{MultipleSelect}}", new MultipleSelect().ToHtml());
                html = html.Replace("{{Tags}}", new HtmlTag().ToHtml());
            }

            if (htmlName == HtmlFile.ScheduleHtml)
            {
                html = html.Replace("{{ManualRotatingCards}}", new SchedulePage().ToHtml());
            }

            return this.HtmlResult(html);
        }

        /// <summary>
        /// 获取所有Job（详情信息 - 初始化页面调用）.
        /// </summary>
        /// <returns></returns>
        protected async Task<List<TaskScheduleModel>> GetAllJobAsync(List<TaskScheduleModel> list)
        {
            List<JobKey> jboKeyList = new List<JobKey>();
            List<TaskScheduleModel> newList = new List<TaskScheduleModel>();
            var groupNames = await this.scheduler.GetJobGroupNames();
            foreach (var groupName in groupNames.OrderBy(t => t))
            {
                jboKeyList.AddRange(await this.scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)));
            }

            foreach (var jobKey in jboKeyList.OrderBy(t => t.Name))
            {
                var jobDetail = await this.scheduler.GetJobDetail(jobKey);
                var triggersList = await this.scheduler.GetTriggersOfJob(jobKey);
                var triggers = triggersList.AsEnumerable().FirstOrDefault();

                var interval = string.Empty;
                if (triggers is SimpleTriggerImpl)
                {
                    interval = (triggers as SimpleTriggerImpl)?.RepeatInterval.ToString();
                }
                else
                {
                    interval = (triggers as CronTriggerImpl)?.CronExpressionString;
                }

                var item = list.FirstOrDefault(o => o.Name == jobKey.Name && o.Group == jobKey.Group);
                if (item != null)
                {
                    item.Name = jobKey.Name;
                    item.Group = jobKey.Group;
                    item.ExceptionMessage = jobDetail.JobDataMap.GetString(Constant.Exception);
                    item.Url = jobDetail.JobDataMap.GetString(Constant.RequestUrl);
                    item.Status = await this.scheduler.GetTriggerState(triggers.Key);
                    item.PrevFireTime = triggers.GetPreviousFireTimeUtc()?.LocalDateTime;
                    item.NextFireTime = triggers.GetNextFireTimeUtc()?.LocalDateTime;
                    item.StartTime = triggers.StartTimeUtc.LocalDateTime;
                    item.CronExpression = interval;
                    item.EndTime = triggers.EndTimeUtc?.LocalDateTime;
                    item.Description = jobDetail.Description;

                    newList.Add(item);
                }
            }

            return newList;
        }
    }
}

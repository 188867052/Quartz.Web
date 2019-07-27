namespace Quartz.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Quartz.Entity;
    using Quartz.Files;
    using Quartz.Framework;
    using Quartz.Html;

    [ResponseCache(Duration = 3600)]
    public class HomeController : StandardController
    {
        public HomeController(IGetHtml getHtml, QuartzContext dbContext)
            : base( dbContext)
        {
            this.getHtml = getHtml;
        }

        public IActionResult Presentation()
        {
            return this.CommonReplaces(HtmlFile.PresentationHtml);
        }

        [ResponseCache(Duration = 3600)]
        public IActionResult Icons()
        {
            return this.Page(new IconPage());
        }

        public IActionResult SignUp()
        {
            return this.CommonReplaces(HtmlFile.SignupPageHtml);
        }

        public IActionResult Components()
        {
            return this.CommonReplaces(HtmlFile.IndexHtml);
        }

        public IActionResult AboutUs()
        {
            return this.CommonReplaces(HtmlFile.AboutUsHtml);
        }

        public IActionResult ContactUs()
        {
            return this.CommonReplaces(HtmlFile.ContactUsHtml);
        }

        public IActionResult PricingPage()
        {
            return this.CommonReplaces(HtmlFile.PricingHtml);
        }

        public IActionResult ProfilePage()
        {
            return this.CommonReplaces(HtmlFile.ProfilePageHtml);
        }

        public IActionResult Ecommerce()
        {
            return this.CommonReplaces(HtmlFile.EcommerceHtml);
        }

        public IActionResult BlogPost()
        {
            return this.CommonReplaces(HtmlFile.BlogPostHtml);
        }

        public IActionResult BlogPosts()
        {
            return this.CommonReplaces(HtmlFile.BlogPostsHtml);
        }

        public IActionResult LandingPage()
        {
            return this.CommonReplaces(HtmlFile.LandingPageHtml);
        }

        public IActionResult LoginPage()
        {
            return this.CommonReplaces(HtmlFile.LoginPageHtml);
        }

        public IActionResult ProductPage()
        {
            return this.CommonReplaces(HtmlFile.ProductPageHtml);
        }

        public IActionResult Headers()
        {
            return this.CommonReplaces(HtmlFile.SectionsHtml);
        }

        public IActionResult Features()
        {
            return this.CommonReplaces(HtmlFile.SectionsHtml);
        }

        public IActionResult Blogs()
        {
            return this.CommonReplaces(HtmlFile.SectionsHtml);
        }

        public IActionResult Teams()
        {
            return this.CommonReplaces(HtmlFile.SectionsHtml);
        }

        public IActionResult Projects()
        {
            return this.CommonReplaces(HtmlFile.SectionsHtml);
        }

        public IActionResult Pricing()
        {
            return this.CommonReplaces(HtmlFile.SectionsHtml);
        }

        public IActionResult Testimonials()
        {
            return this.CommonReplaces(HtmlFile.SectionsHtml);
        }

        public IActionResult Contacts()
        {
            return this.CommonReplaces(HtmlFile.ContactUsHtml);
        }
    }
}

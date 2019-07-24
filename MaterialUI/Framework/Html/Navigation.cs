namespace MaterialUI.Html
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using MaterialUI.Html.DropDowns;
    using MaterialUI.Html.Tags;
    using MaterialUI.Routes;
    using Route.Generator;
    using TagHelper = AspNetCore.Extensions.TagHelper;

    public static class Navigation
    {
        private static string navCache;

        public static string GetNavbar()
        {
            if (!string.IsNullOrEmpty(navCache))
            {
                return navCache;
            }

            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "container"), Navigation.GetNavbarHeader() + Navigation.GetNavbarBody());
            var nav = TagHelper.Create(Tag.nav, new TagAttribute(Attr.Class, "navbar navbar-primary navbar-transparent navbar-absolute"), div);
            navCache = TagHelper.ToHtml(nav);
            return navCache;
        }

        private static string GetNavbarHeader()
        {
            TagAttributeList attributes = new TagAttributeList
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Class, "navbar-toggle"),
                new TagAttribute(Attr.DataToggle, "collapse"),
                new TagAttribute(Attr.DataTarget, "#navigation-example"),
            };
            var aAttributes = new TagAttributeList
            {
                new TagAttribute(Attr.Class, "navbar-brand"),
                new TagAttribute(Attr.Href, HomeRoute.Presentation),
            };
            var span = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "sr-only"), "Toggle navigation");
            var span1 = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "icon-bar"));
            var a = TagHelper.Create(Tag.a, aAttributes, "Material Kit Pro");
            var button = TagHelper.Create(Tag.button, attributes, span, span1, span1, span1);
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "navbar-header"), button, a);
            return TagHelper.ToHtml(div);
        }

        private static string GetDropDown()
        {
            DropDownMenu menu1 = new DropDownMenu(HomeRoute.Components, "apps", "Components");
            IList<DropDownMenu> list1 = new List<DropDownMenu>
            {
                new DropDownMenu($"{HomeRoute.Headers}#headers", "dns", "Headers"),
                new DropDownMenu($"{HomeRoute.Features}#features", "build", "Features"),
                new DropDownMenu($"{HomeRoute.Blogs}#blogs", "list", "Blogs"),
                new DropDownMenu($"{HomeRoute.Teams}#teams", "people", "Teams"),
                new DropDownMenu($"{HomeRoute.Projects}#projects", "assignment", "Projects"),
                new DropDownMenu($"{HomeRoute.Pricing}#pricing", "monetization_on", "Pricing"),
                new DropDownMenu($"{HomeRoute.Testimonials}#testimonials", "chat", "Testimonials"),
                new DropDownMenu($"{HomeRoute.ContactUs}#contactus", "call", "Contacts"),
            };

            DropDown dropDown1 = new DropDown
            {
                Toggle = new DropDownToggle("view_day", "Sections"),
                Menu = list1,
            };

            IList<DropDownMenu> list2 = new List<DropDownMenu>
            {
                new DropDownMenu(HomeRoute.AboutUs, "account_balance", "About Us"),
                new DropDownMenu(HomeRoute.BlogPost, "art_track", "Blog Post"),
                new DropDownMenu(HomeRoute.BlogPosts, "view_quilt", "Blog Posts"),
                new DropDownMenu(HomeRoute.ContactUs, "location_on", "Contact Us"),
                new DropDownMenu(HomeRoute.LandingPage, "view_day", "Landing Page"),
                new DropDownMenu(HomeRoute.LoginPage, "fingerprint", "Login Page"),
                new DropDownMenu(HomeRoute.PricingPage, "attach_money", "Pricing Page"),
                new DropDownMenu(HomeRoute.Ecommerce, "shop", "Ecommerce Page"),
                new DropDownMenu(HomeRoute.ProductPage, "beach_access", "Product Page"),
                new DropDownMenu(HomeRoute.ProfilePage, "account_circle", "Profile Page"),
                new DropDownMenu(HomeRoute.SignUp, "person_add", "Signup Page"),
                new DropDownMenu(HomeRoute.Icons, "insert_emoticon", "Material Icons"),
                new DropDownMenu(ScheduleRoute.Index, "schedule", "Task Schedules"),
            };

            DropDown dropDown2 = new DropDown
            {
                Toggle = new DropDownToggle("view_carousel", "Examples"),
                Menu = list2,
            };

            IList<DropDownMenu> list3 = new List<DropDownMenu>
            {
                new DropDownMenu(GeneratorRoute.ShowClass, "class", "Route Class"),
                new DropDownMenu(RouteRoute.Index, "ac_unit", "Route Index"),
                new DropDownMenu(RouteRoute.ShowAllRoutes, "smoking_rooms", "Route Json"),
                new DropDownMenu(GeneratorRoute.GenerateEnum, "smoking_rooms", "Generate Enums"),
                new DropDownMenu(GeneratorRoute.RenameFile, "smoking_rooms", "Rename Files"),
                new DropDownMenu(DependencyInjection.Analyzer.RouteRoute.ShowAllServices, "pool", "Dependency Injection"),
                new DropDownMenu(GeneratorRoute.ShowScript, "style", "Script & Style & Html"),
                new DropDownMenu(GeneratorRoute.AppSettings, "settings", "Show AppSettings"),
                new DropDownMenu(GeneratorRoute.TagAnalyze, "check", "Tag Analyze"),
                new DropDownMenu(GeneratorRoute.SortingTag, "sort_by_alpha", "Sort Tag Fields"),
                new DropDownMenu(EntityFrameworkCore.SqlProfile.RouteRoute.ShowAllSql, "search", "Sql Profile"),
            };

            DropDown dropDown3 = new DropDown
            {
                Toggle = new DropDownToggle("flash_auto", "Generator"),
                Menu = list3,
            };

            IList<DropDownMenu> list4 = new List<DropDownMenu>
            {
                new DropDownMenu("https://material.io/tools/icons/?style=baseline", "insert_emoticon", "Material Icons"),
                new DropDownMenu("http://www.fontawesome.com.cn/faicons/", "insert_emoticon", "Font Awesome Icons"),
                new DropDownMenu(GeneratorRoute.Scaffold, "insert_emoticon", "Scaffold"),
            };

            DropDown dropDown4 = new DropDown
            {
                Toggle = new DropDownToggle("link", "Links"),
                Menu = list4,
            };

            return menu1.ToHtml() + dropDown1.ToHtml() + dropDown2.ToHtml() + dropDown3.ToHtml() + dropDown4.ToHtml();
        }

        private static string GetNavbarBody()
        {
            var ul = TagHelper.Create(Tag.ul, new TagAttribute(Attr.Class, "nav navbar-nav navbar-right"), Navigation.GetDropDown());
            var div = TagHelper.Div("collapse navbar-collapse", ul);
            return TagHelper.ToHtml(div);
        }
    }
}

namespace Quartz.Web.Test
{
    using System;
    using System.Collections.Generic;
    using Quartz.Entity;
    using Quartz.Html.Buttons;
    using Quartz.Html.Checkbox;
    using Quartz.Html.Dialog.Demo;
    using Quartz.Html.Inputs;
    using Quartz.Html.RadioButtons;
    using Quartz.Html.Tables;
    using Xunit;

    public class HtmlUnitTest
    {
        [Fact]
        public void ManualRotatingCards()
        {
            var log = Dapper.DapperExtension.Find<QuartzLog>(1);

            var modal = new ManualRotatingCard();
            var a = modal.ToHtml();
        }

        [Fact]
        public void Table()
        {
            var modal = new Table();
            var a = modal.ToHtml();
        }

        [Fact]
        public void Tooltip()
        {
            var modal = new Tooltip("On left", "left", "Tooltip on left");
            var a = modal.ToHtml();
        }

        [Fact]
        public void Popovers()
        {
            var modal = new Popovers("On left", "left", "Popovers on left", "Here will be some very useful information about his popover.<br> Here will be some very useful information about his popover.");
            var a = modal.ToHtml();
        }

        [Fact]
        public void ButtonTest()
        {
            var modal = Button.Generate();
        }

        [Fact]
        public void InputTest()
        {
            var modal = new Input();
            var a = modal.ToHtml();
        }

        [Fact]
        public void LabeledInputTest()
        {
            var modal = new LabeledInput();
            var a = modal.ToHtml();
        }

        [Fact]
        public void LabeledSuccessInput()
        {
            var modal = new LabeledSuccessInput();
            var a = modal.ToHtml();
        }

        [Fact]
        public void LabeledErrorInput()
        {
            var modal = new LabeledErrorInput();
            var a = modal.ToHtml();
        }

        [Fact]
        public void MaterialIconInput()
        {
            var modal = new MaterialIconInput();
            var a = modal.ToHtml();
        }

        [Fact]
        public void FontAwesomeIconInput()
        {
            var modal = new FontAwesomeIconInput();
            var a = modal.ToHtml();
        }

        [Fact]
        public void CheckboxTest()
        {
            var modal = Checkbox.Generate();
        }

        [Fact]
        public void RadioButtonTest()
        {
            var modal = RadioButton.Generate();
        }

        [Fact]
        public void ToggleButtonTest()
        {
            var modal = ToggleButton.Generate();
        }

        [Fact]
        public void Dropdown()
        {
            var modal = new Dropdown();
            var a = modal.ToHtml();
        }

        [Fact]
        public void Textarea()
        {
            var modal = new Textarea();
            var a = modal.ToHtml();
        }

        [Fact]
        public void SelectPicker()
        {
            var modal = new SingleSelect();
            var a = modal.ToHtml();
        }

        [Fact]
        public void HtmlTag()
        {
            var modal = new HtmlTag();
            var a = modal.ToHtml();
        }

        [Fact]
        public void MultipleSelect()
        {
            var modal = new MultipleSelect();
            var a = modal.ToHtml();
            Console.WriteLine(a);
        }

        [Fact]
        public void HtmlIconsTest()
        {
            var modal = HtmlIcons.Generate(new List<Icon> { new Icon { Name = "verified_user", Id = 1 }, new Icon { Name = "verified_user", Id = 1 } });
        }
    }
}

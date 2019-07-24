namespace XUnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MaterialUI.Entity;
    using MaterialUI.GridConfigurations.Schedule;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Html.Checkbox;
    using MaterialUI.Html.Dialog.Demo;
    using MaterialUI.Html.Inputs;
    using MaterialUI.Html.RadioButtons;
    using MaterialUI.Html.Tables;
    using Xunit;

    public class HtmlUnitTest
    {
        [Fact]
        public void ManualRotatingCards()
        {
            string str = $@"                entity.Property(e => e.Name)
                    .HasName(Icon_name_unique)
                    .IsUnique(); ";
            string begin = "entity.Property(e => e.";
            string end = "Icon_name_unique";
            int beginIndex = str.IndexOf(begin) + begin.Length;
            int endIndex = str.IndexOf(end);
            string resultstr = str.Substring(beginIndex, endIndex - beginIndex);

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

        [Fact]
        public void Schedule()
        {
            MaterialUIContext dbContext = new MaterialUIContext();
            var model = dbContext.TaskSchedule.Take(1).ToList();
            ScheduleGridConfiguration<TaskSchedule> grid = new ScheduleGridConfiguration<TaskSchedule>(model);
            var html = grid.Render();
        }
    }
}

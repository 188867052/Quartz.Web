namespace MaterialKit.Javascript
{
    using System;

    public static class EnumMapping
    {
        /// <summary>
        /// Text box type enum.
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>string</returns>
        public static string ToString(TextBoxTypeEnum type)
        {
            switch (type)
            {
                case TextBoxTypeEnum.Text:
                    return "text";
                case TextBoxTypeEnum.Password:
                    return "password";
                case TextBoxTypeEnum.Hidden:
                    return "hidden";
                default:
                    throw new Exception("error type.");
            }
        }

        /// <summary>
        /// Java script event enum.
        /// </summary>
        /// <param name="event">event.</param>
        /// <returns>string</returns>
        public static string ToString(JavaScriptEventEnum @event)
        {
            switch (@event)
            {
                case JavaScriptEventEnum.Click:
                    return "click";
                case JavaScriptEventEnum.Blur:
                    return "blur";
                case JavaScriptEventEnum.Change:
                    return "change";
                case JavaScriptEventEnum.Focus:
                    return "focus";
                case JavaScriptEventEnum.Input:
                    return "input";
                case JavaScriptEventEnum.MouseDown:
                    return "mousedown";
                default:
                    throw new Exception("error type.");
            }
        }
    }
}

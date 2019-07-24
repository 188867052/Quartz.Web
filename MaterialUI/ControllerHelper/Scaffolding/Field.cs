namespace MaterialUI.Scaffolding
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Field
    {
        [XmlAttribute("summary")]
        public string Summary { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "iskey")]
        [DefaultValue(false)]
        public bool IsKey { get; set; }

        [XmlAttribute("conversion")]
        public string HasConversion { get; set; }
    }
}
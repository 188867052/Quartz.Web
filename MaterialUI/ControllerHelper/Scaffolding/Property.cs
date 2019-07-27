namespace Quartz.Scaffolding
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Property
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

        [XmlAttribute("hasConversion")]
        [DefaultValue(false)]
        public bool HasConversion { get; set; }

        [XmlAttribute("dbType")]
        public string DbType { get; set; }

        [XmlAttribute("csharpType")]
        public string CSharpType { get; set; }
    }
}
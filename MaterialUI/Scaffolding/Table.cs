namespace MaterialKit.Scaffolding
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Table
    {
        [XmlElement(ElementName = "field")]
        public Field[] Fields { get; set; }

        [XmlAttribute("summary")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
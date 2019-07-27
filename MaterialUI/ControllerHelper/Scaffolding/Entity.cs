namespace Quartz.Scaffolding
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Entity
    {
        [XmlElement(ElementName = "property")]
        public Property[] Properties { get; set; }

        [XmlAttribute("summary")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
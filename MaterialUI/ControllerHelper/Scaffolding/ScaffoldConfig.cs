namespace MaterialUI.Scaffolding
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "config", IsNullable = false)]
    public class ScaffoldConfig
    {
        [XmlElement("table")]
        public Table[] Tables { get; set; }

        [XmlElement("namespace")]
        public Namespace[] Namespaces { get; set; }
    }
}
namespace trx_tools.Core.Models;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class Times
{
    [System.Xml.Serialization.XmlAttributeAttribute("creation")]
    public required DateTime Creation { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("queuing")]
    public required DateTime Queuing { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("start")]
    public required DateTime Start { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("finish")]
    public required DateTime Finish { get; set; }
}
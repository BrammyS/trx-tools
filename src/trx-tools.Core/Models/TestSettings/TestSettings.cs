namespace trx_tools.Core.Models.TestSettings;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class TestSettings
{
    public required Deployment Deployment { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("name")]
    public required string Name { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("id")]
    public required string Id { get; set; }
}
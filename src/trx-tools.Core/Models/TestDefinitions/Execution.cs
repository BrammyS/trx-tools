namespace trx_tools.Core.Models.TestDefinitions;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class Execution
{
    [System.Xml.Serialization.XmlAttributeAttribute("id")]
    public required string Id { get; set; }
}
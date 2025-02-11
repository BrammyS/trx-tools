namespace trx_tools.Core.Models.TestDefinitions;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class UnitTest
{
    public required Execution Execution { get; set; }

    public required TestMethod TestMethod { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("name")]
    public required string Name { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("storage")]
    public required string Storage { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("id")]
    public required string Id { get; set; }
}
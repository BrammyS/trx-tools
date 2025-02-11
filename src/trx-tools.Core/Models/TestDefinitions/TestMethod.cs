namespace trx_tools.Core.Models.TestDefinitions;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class TestMethod
{
    [System.Xml.Serialization.XmlAttributeAttribute("codeBase")]
    public required string CodeBase { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("adapterTypeName")]
    public required string AdapterTypeName { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("className")]
    public required string ClassName { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("name")]
    public required string Name { get; set; }
}
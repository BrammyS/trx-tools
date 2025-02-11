namespace trx_tools.Core.Models.TestEntries;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class TestEntry
{
    [System.Xml.Serialization.XmlAttributeAttribute("testId")]
    public required string TestId { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("executionId")]
    public required string ExecutionId { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("testListId")]
    public required string TestListId { get; set; }
}
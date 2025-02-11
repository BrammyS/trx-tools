namespace trx_tools.Core.Models.ResultSummary;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class RunInfo
{
    public required string Text { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("computerName")]
    public required string ComputerName { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("outcome")]
    public required string Outcome { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("timestamp")]
    public DateTime Timestamp { get; set; }
}
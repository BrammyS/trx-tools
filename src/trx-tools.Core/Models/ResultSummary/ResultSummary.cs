namespace trx_tools.Core.Models.ResultSummary;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class ResultSummary
{
    public required Counters Counters { get; set; }

    public required Output Output { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("outcome")]
    public required string Outcome { get; set; }

    [System.Xml.Serialization.XmlArrayItemAttribute("RunInfo", IsNullable = false)]
    public required RunInfo[] RunInfos { get; set; }
}
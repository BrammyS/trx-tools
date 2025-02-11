using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.ResultSummary;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class RunInfo
{
    public required string Text { get; set; }

    [XmlAttribute("computerName")] public required string ComputerName { get; set; }

    [XmlAttribute("outcome")] public required string Outcome { get; set; }

    [XmlAttribute("timestamp")] public DateTime Timestamp { get; set; }
}
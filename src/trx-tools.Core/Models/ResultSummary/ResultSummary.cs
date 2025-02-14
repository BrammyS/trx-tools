using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.ResultSummary;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class ResultSummary
{
    public required Counters Counters { get; set; }

    public required Output? Output { get; set; }

    [XmlAttribute("outcome")] public required string Outcome { get; set; }

    [XmlArrayItem("RunInfo", IsNullable = false)]
    public required RunInfo[] RunInfos { get; set; }
}
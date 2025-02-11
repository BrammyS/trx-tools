using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.ResultSummary;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class Output
{
    public required string StdOut { get; set; }
}
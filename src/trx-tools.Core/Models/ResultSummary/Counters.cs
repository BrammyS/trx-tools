using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.ResultSummary;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class Counters
{
    [XmlAttribute("total")] public uint Total { get; set; }

    [XmlAttribute("executed")] public uint Executed { get; set; }

    [XmlAttribute("passed")] public uint Passed { get; set; }

    [XmlAttribute("failed")] public uint Failed { get; set; }

    [XmlAttribute("error")] public uint Error { get; set; }

    [XmlAttribute("timeout")] public uint Timeout { get; set; }

    [XmlAttribute("aborted")] public uint Aborted { get; set; }

    [XmlAttribute("inconclusive")] public uint Inconclusive { get; set; }

    [XmlAttribute("passedButRunAborted")] public uint PassedButRunAborted { get; set; }

    [XmlAttribute("notRunnable")] public uint NotRunnable { get; set; }

    [XmlAttribute("notExecuted")] public uint NotExecuted { get; set; }

    [XmlAttribute("disconnected")] public uint Disconnected { get; set; }

    [XmlAttribute("warning")] public uint Warning { get; set; }

    [XmlAttribute("completed")] public uint Completed { get; set; }

    [XmlAttribute("inProgress")] public uint InProgress { get; set; }

    [XmlAttribute("pending")] public uint Pending { get; set; }
}
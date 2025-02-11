namespace trx_tools.Core.Models.ResultSummary;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class Counters
{
    [System.Xml.Serialization.XmlAttributeAttribute("total")]
    public uint Total { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("executed")]
    public uint Executed { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("passed")]
    public uint Passed { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("failed")]
    public uint Failed { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("error")]
    public uint Error { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("timeout")] 
    public uint Timeout { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("aborted")]
    public uint Aborted { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("inconclusive")]
    public uint Inconclusive { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("passedButRunAborted")]
    public uint PassedButRunAborted { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("notRunnable")]
    public uint NotRunnable { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("notExecuted")]
    public uint NotExecuted { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("disconnected")]
    public uint Disconnected { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("warning")]
    public uint Warning { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("completed")]
    public uint Completed { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("inProgress")]
    public uint InProgress { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("pending")]
    public uint Pending { get; set; }
}
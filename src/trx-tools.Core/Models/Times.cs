using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class Times
{
    [XmlAttribute("creation")] public required DateTime Creation { get; set; }

    [XmlAttribute("queuing")] public required DateTime Queuing { get; set; }

    [XmlAttribute("start")] public required DateTime Start { get; set; }

    [XmlAttribute("finish")] public required DateTime Finish { get; set; }
}
using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.TestDefinitions;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class Execution
{
    [XmlAttribute("id")] public required string Id { get; set; }
}
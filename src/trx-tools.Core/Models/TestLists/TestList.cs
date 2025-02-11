using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.TestLists;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class TestList
{
    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlAttribute("id")] public required string Id { get; set; }
}